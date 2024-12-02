using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidObjectSwimmingBehavior : MonoBehaviour
{
   
   [SerializeField] private float _obstacleDistance = 2;
   [SerializeField] private float _obstacleWeight = 50;
   [SerializeField] private float _movementSmoothDamp = 0.5f;
   [SerializeField] private float _directionResetInterval = 1f;
   [SerializeField] private Transform _boundCenter;
   [SerializeField] private Vector3 _bound;
   
   private Vector3[] _directionToCheckWithObstacle;
   private Vector3 _currentVelocity;
   private Vector3 _currentObstacleAvoidanceVelocity;
   private Transform _myTransform;
   private Vector3 _moveVector;
   private float _changeDirectionTimer;
   
    // Start is called before the first frame update
    void Start()
    {
       _myTransform = transform;
       _directionToCheckWithObstacle = CalculatedDir();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float speed)
    {
       //CheckBounds();
       _changeDirectionTimer--;
       Vector3 randomDirectionVector;
       if (_changeDirectionTimer < 0)
          randomDirectionVector = GetRandomDirection();
       else
          randomDirectionVector = Vector3.zero;

       Vector3 obstacleAvoidVector = CalculateObstacleAvoid(speed) * _obstacleWeight;
       _moveVector = randomDirectionVector + obstacleAvoidVector;
       
       _moveVector = Vector3.SmoothDamp(_myTransform.forward, _moveVector, ref _currentVelocity,
          _movementSmoothDamp);
       //move the unit
       _moveVector = _moveVector.normalized * speed;
       _myTransform.forward = _moveVector;
       _myTransform.position += _moveVector * Time.deltaTime;
    }
    
    private Vector3 GetRandomDirection()
    {
       // Generate a random direction
       Vector3 targetDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

       // Reset the direction change timer
       _changeDirectionTimer = _directionResetInterval;
       return targetDirection;
    }

    private void CheckBounds()
    {
       Vector3 pos = _myTransform.position;

       Vector3 globalBound = _bound + _boundCenter.position;
       // Clamp position to stay within the defined bounds
       if (pos.x < -globalBound.x || pos.x > globalBound.x ||
           pos.y < -globalBound.y || pos.y > globalBound.y ||
           pos.z < -globalBound.z || pos.z > globalBound.z)
       {
          // Teleport the object back into bounds
          pos.x = Mathf.Clamp(pos.x, -globalBound.x, globalBound.x);
          pos.y = Mathf.Clamp(pos.y, -globalBound.y, globalBound.y);
          pos.z = Mathf.Clamp(pos.z, -globalBound.z, globalBound.z);
          _myTransform.position = pos;
       }
    }
    private Vector3 CalculateObstacleAvoid(float speed) 
    {
       Vector3 obstacleVector = Vector3.zero;
      RaycastHit hit;
      //have a obstacle or not
      if(Physics.Raycast(_myTransform.position,_myTransform.forward,out hit, 
            _obstacleDistance))
      {
         obstacleVector = FindBestDirectionToAvoid(speed);
      }
      else
      {
         //if no obstacle
         _currentObstacleAvoidanceVelocity = Vector3.zero;
      }
      
      return obstacleVector;
    }
    
   //find the best by casting 4 raycast choose the ray which not hit to move or if all ray hit choose the longest one
   private Vector3 FindBestDirectionToAvoid(float speed)
   {
      if (_currentObstacleAvoidanceVelocity != Vector3.zero)
      {
         RaycastHit hit;
         //recheck if noobstacle
         if(!Physics.Raycast(_myTransform.position,_myTransform.forward,out hit, 
               _obstacleDistance))
         {
            return _currentObstacleAvoidanceVelocity;
         }
      }
      
      float maxDistance = int.MinValue;
      Vector3 selectedDir = Vector3.zero;
      
      for (int i = 0; i < _directionToCheckWithObstacle.Length; i++)
      {
         RaycastHit hit;
         //convert wolrdspace dir to local dir
         var currentDir = _myTransform.TransformDirection(_directionToCheckWithObstacle[i].normalized);
         //if it hit
         if (Physics.Raycast(_myTransform.position, currentDir,out hit, _obstacleDistance))
         {
            
            //calculate the hit length
            float currentDistance = (hit.point - _myTransform.position).sqrMagnitude;
            //compare
            if (currentDistance > maxDistance)
            {
               maxDistance = currentDistance;
               selectedDir = currentDir;
            }
         }
         else
         {
            selectedDir = currentDir;
            Vector3 forward = _myTransform.forward;
            _currentObstacleAvoidanceVelocity = currentDir.normalized*speed-forward;
            return Vector3.ClampMagnitude(selectedDir.normalized*speed-forward,4);
         }
      }

      return  Vector3.ClampMagnitude(selectedDir.normalized*speed-_myTransform.forward,4);
   }

   private Vector3[] CalculatedDir()
   {
      int numViewDirections = 300;
      Vector3[] directions = new Vector3[numViewDirections];

      float goldenRatio = (1 + Mathf.Sqrt (5)) / 2;
      float angleIncrement = Mathf.PI * 2 * goldenRatio;

      for (int i = 0; i < numViewDirections; i++) {
         float t = (float) i / numViewDirections;
         float inclination = Mathf.Acos (1 - 2 * t);
         float azimuth = angleIncrement * i;

         float x = Mathf.Sin (inclination) * Mathf.Cos (azimuth);
         float y = Mathf.Sin (inclination) * Mathf.Sin (azimuth);
         float z = Mathf.Cos (inclination);
         directions[i] = new Vector3 (x, y, z);
      }

      return directions;
   }
}
