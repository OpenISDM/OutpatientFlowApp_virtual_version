﻿/*
 * Copyright (c) 2019 Academia Sinica, Institude of Information Science
 *
 * License:
 *      GPL 3.0 : The content of this file is subject to the terms and
 *      conditions defined in file 'COPYING.txt', which is part of this source
 *      code package.
 *
 * Project Name:
 *
 *      IndoorNavigation
 *
 * File Description:
 * 
 *      
 *      
 * Version:
 *
 *      1.0.0, 20190719
 * 
 * File Name:
 *
 *      IBeaconClient.cs
 *
 * Abstract:
 *
 *      This file will call the bluetooth scanning code in both
 *      IOS and Android. In IBeaconClient.cs, we will store all the
 *      signals we get in the buffer. And we will cancel some signals
 *      that their threshold are too low. Yhen we calculate the threshold
 *      and get the average.
 *
 * Authors:
 *
 *      Eric Lee, ericlee@iis.sinica.edu.tw
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using IndoorNavigation.Models;
using Xamarin.Forms;

namespace IndoorNavigation.Modules.IPSClients
{
    class IBeaconClient : IIPSClient
    {
        private List<WaypointBeaconsMapping> _waypointBeaconsList = new List<WaypointBeaconsMapping>();
        private const int _clockResetTime = 90000;
        private object _bufferLock;// = new object();
        private readonly EventHandler _beaconScanEventHandler;
        //private Dictionary<string, Ibea>
        public NavigationEvent _event { get; private set; }
        private const int _moreThanTwoIBeacon = 2;
        private List<BeaconSignalModel> _beaconSignalBuffer = new List<BeaconSignalModel>();
        private int rssiOption;
        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        public IBeaconClient()
        {
            Console.WriteLine("In Ibeacon Type");

            _event = new NavigationEvent();
            Utility._ibeaconScan = DependencyService.Get<IBeaconScan>();
            _bufferLock = new object();
            _beaconScanEventHandler = new EventHandler(HandleBeaconScan);
            Utility._ibeaconScan._event._eventHandler += _beaconScanEventHandler;
            _waypointBeaconsList = new List<WaypointBeaconsMapping>();
            rssiOption = 0;
            
            watch.Start();
        }
        public void SetWaypointList(List<WaypointBeaconsMapping> waypointBeaconsList)
        {
            
            if (Application.Current.Properties.ContainsKey("StrongRssi"))
            {
                if ((bool)Application.Current.Properties["StrongRssi"] == true)
                {
                    rssiOption = 5;
                }
                else if ((bool)Application.Current.Properties["MediumRssi"] == true)
                {
                    rssiOption = 0;
                }
                else if ((bool)Application.Current.Properties["WeakRssi"] == true)
                {
                    rssiOption = -2;
                }
            }

            this._waypointBeaconsList = waypointBeaconsList;
            Utility._ibeaconScan.StartScan();
        }

        public void DetectWaypoints()
        {
    
            List<BeaconSignalModel> removeSignalBuffer =
                new List<BeaconSignalModel>();
            
            lock (_bufferLock)
            {
                removeSignalBuffer.AddRange(
                   _beaconSignalBuffer.Where(c =>
                   c.Timestamp < DateTime.Now.AddMilliseconds(-1500)));

                foreach (var obsoleteBeaconSignal in removeSignalBuffer)
                    _beaconSignalBuffer.Remove(obsoleteBeaconSignal);

                Dictionary<RegionWaypointPoint, List<BeaconSignal>> scannedData = new Dictionary<RegionWaypointPoint, List<BeaconSignal>>();

                Dictionary<RegionWaypointPoint, int> signalAvgValue = new Dictionary<RegionWaypointPoint, int>();

                Dictionary<RegionWaypointPoint, List<BeaconSignal>> correctData = new Dictionary<RegionWaypointPoint, List<BeaconSignal>>();

                
                if (watch.Elapsed.TotalMilliseconds >= _clockResetTime)
                {
                    watch.Stop();
                    watch.Reset();
                    watch.Start();
                    Utility._ibeaconScan.StopScan();
                    Utility._ibeaconScan.StartScan();

                }


                //In ibsclient, a waypoint has at least two beacon UUIDs,
                //We put all waypoint we get in scannedData

                //BeaconSignalModel beaconSignalModel1 = new BeaconSignalModel();
                //BeaconSignalModel beaconSignalModel2 = new BeaconSignalModel();
                //BeaconSignalModel beaconSignalModel3 = new BeaconSignalModel();
                //BeaconSignalModel beaconSignalModel4 = new BeaconSignalModel();
                //beaconSignalModel1.UUID = new Guid("00000000-0402-5242-3d64-00cdff001d09");
                //beaconSignalModel2.UUID = new Guid("00000000-0402-5242-3d64-2019010044a4");
                //beaconSignalModel3.UUID = new Guid("00000000-0402-5242-3d64-2019010047b2");
                //beaconSignalModel4.UUID = new Guid("00000000-0402-5242-3d64-2019010049bf");
                //beaconSignalModel1.RSSI = -50;
                //beaconSignalModel2.RSSI = -60;
                //beaconSignalModel3.RSSI = -55;
                //beaconSignalModel4.RSSI = -33;
                //_beaconSignalBuffer.Add(beaconSignalModel1);
                //_beaconSignalBuffer.Add(beaconSignalModel2);
                //_beaconSignalBuffer.Add(beaconSignalModel3);
                //_beaconSignalBuffer.Add(beaconSignalModel4);

                //Mapping the interested beacon and cancel the beacon which has too low threshold
                foreach (BeaconSignalModel beacon in _beaconSignalBuffer)
                {
                    foreach (WaypointBeaconsMapping waypointBeaconsMapping in _waypointBeaconsList)
                    {
                        foreach (Guid beaconGuid in waypointBeaconsMapping._Beacons)
                        {
                            if ((beacon.UUID.Equals(beaconGuid))&&(beacon.RSSI>(waypointBeaconsMapping._BeaconThreshold[beacon.UUID]-rssiOption)))
                            {
                                if (!scannedData.Keys.Contains(waypointBeaconsMapping._WaypointIDAndRegionID))
                                {
                                    scannedData.Add(waypointBeaconsMapping._WaypointIDAndRegionID, new List<BeaconSignal>{ beacon});
                                }
                                else
                                {
                                    scannedData[waypointBeaconsMapping._WaypointIDAndRegionID].Add(beacon);
                                }
                            }
                        }  
                    }
                }

                //Make sure we have got more than two interested guid in each waypoint
                foreach(KeyValuePair < RegionWaypointPoint, List < BeaconSignal >>interestedBeacon in scannedData)
                {
                    //Console.WriteLine("Key : " + interestedBeacon.Value);
                    Dictionary<Guid, List<BeaconSignal>> tempSave = new Dictionary<Guid, List<BeaconSignal>>();

                    foreach(BeaconSignal beaconSignal in interestedBeacon.Value)
                    {

                        if(!tempSave.Keys.Contains(beaconSignal.UUID))
                        {
                            tempSave.Add(beaconSignal.UUID, new List<BeaconSignal> { beaconSignal });
                        }
                        else
                        {
                            tempSave[beaconSignal.UUID].Add(beaconSignal);
                        }
                    }
                    if(tempSave.Keys.Count()>= _moreThanTwoIBeacon)
                    {
                        correctData.Add(interestedBeacon.Key, interestedBeacon.Value);
                    }

                }

                foreach(KeyValuePair<RegionWaypointPoint, List<BeaconSignal>> calculateData in correctData)
                {
                    //If a waypoint has at least two beacon UUIDs,
                    //this waypoint might be our interested waypoint.
                  
                    if (calculateData.Value.Count()>=_moreThanTwoIBeacon)
                    {
                        Dictionary<Guid, List<int>> saveEachBeacons = new Dictionary<Guid, List<int>>();
                        //Sort the beacons by their Rssi
                        calculateData.Value.Sort((x, y) => { return x.RSSI.CompareTo(y.RSSI); });
                        int avgSignal = 0;
                        //int averageSignal = 0;
                        //List<int> signalOfEachBeacon = new List<int>();
                        //If we have more than ten data, we remove the highest 10%
                        //and the lowest 10%, and calculate their average
                        //If we have not more than 10 data,
                        //we just calculate their average
                        if (calculateData.Value.Count() >= 10)
                        {
                            int min = Convert.ToInt32(scannedData.Count() * 0.1);
                            int max = Convert.ToInt32(scannedData.Count() * 0.9);
                            int minus = max - min;
                           
                            for (int i = min; i < max; i++)
                            {
                                avgSignal += calculateData.Value[i].RSSI;
                            }
                            avgSignal = avgSignal / minus;
                        }
                        else
                        {
                            
                            
                            foreach (BeaconSignal value in calculateData.Value)
                            {
                                avgSignal += value.RSSI;
                            }
                            avgSignal = avgSignal / calculateData.Value.Count();

                        }
                        signalAvgValue.Add(calculateData.Key, avgSignal);
                    }
                }
                
                int tempValue = -100;
                bool haveThing = false;
                RegionWaypointPoint possibleRegionWaypoint = new RegionWaypointPoint();
                //Compare all data we have, and get the highest Rssi Waypoint as our interested waypoint
                foreach(KeyValuePair<RegionWaypointPoint, int> calculateMax in signalAvgValue)
                {
                    if(tempValue<calculateMax.Value)
                    {
                        possibleRegionWaypoint = new RegionWaypointPoint();
                        possibleRegionWaypoint = calculateMax.Key;
                        haveThing = true;
                    }
                    tempValue = calculateMax.Value;
                    
                }

                if(haveThing==true)
                {
                    watch.Stop();
                    watch.Reset();
                    watch.Start();
                    Console.WriteLine("Matched IBeacon");
                    _event.OnEventCall(new WaypointSignalEventArgs
                    {
                        _detectedRegionWaypoint = possibleRegionWaypoint
                    });
                    return;
                }
            }
        }

        private void HandleBeaconScan(object sender, EventArgs e)
        {
            IEnumerable<BeaconSignalModel> signals =
                (e as BeaconScanEventArgs)._signals;

            foreach (BeaconSignalModel signal in signals)
            {
                Console.WriteLine("Detected Beacon UUID : " + signal.UUID + " RSSI = " + signal.RSSI);
            }

            lock (_bufferLock)
                _beaconSignalBuffer.AddRange(signals);

        }

        public void Stop()
        {
            _bufferLock = new object();
            Utility._ibeaconScan.StopScan();
            _beaconSignalBuffer.Clear();
            _waypointBeaconsList.Clear();
            Utility._ibeaconScan._event._eventHandler -= _beaconScanEventHandler;
            watch.Stop();
        }

    }
}
