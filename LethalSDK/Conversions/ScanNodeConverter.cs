using DunGen;
using LethalSDK.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Conversions
{
    public class ScanNodeConverter : ComponentConverter<SI_ScanNode, ScanNodeProperties>
    {
        public override bool ConvertComponent(SI_ScanNode siScanNode)
        {
            ScanNodeProperties scanNodeProperties = siScanNode.gameObject.AddComponent<ScanNodeProperties>();
            scanNodeProperties.minRange = siScanNode.MinRange;
            scanNodeProperties.maxRange = siScanNode.MaxRange;
            scanNodeProperties.requiresLineOfSight = siScanNode.RequiresLineOfSight;
            scanNodeProperties.headerText = siScanNode.HeaderText;
            scanNodeProperties.subText = siScanNode.SubText;
            scanNodeProperties.scrapValue = siScanNode.ScrapValue;
            scanNodeProperties.creatureScanID = siScanNode.CreatureScanID;
            scanNodeProperties.nodeType = (int)siScanNode.NodeType;
            return (true);
        }
    }
}
