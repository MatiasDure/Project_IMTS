using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInfo : MonoBehaviour
{
    [SerializeField] 
        private string _creatureName;
    [SerializeField]
        private Sprite _creatureImage;
	[SerializeField]
		private string _creatureDialogText;

    public string CreatureName => _creatureName;
    public Sprite CreatureImage => _creatureImage;
	public string CreatureDialogText => _creatureDialogText;
}
