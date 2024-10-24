using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlotEventsCollection
{
   [SerializeField] internal Plot _plot;
   [SerializeField] internal List<PlotEvent> _plotEvents;
   
   public List<PlotEvent> PlotEvents => _plotEvents;
   public Plot Plot => _plot; 
}
