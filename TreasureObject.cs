using System;
using System.Collections.Generic;
using Util;

namespace LynnaLib
{
    /// Represents a "treasure object", which is a treasure with additional properties including
    /// graphics, text, etc. (see "data/{game}/treasureObjectData.s".)
    /// Has an "ID" (the treasure index) and an additional "subID" representing a "variant" of the
    /// treasure.
    public class TreasureObject {
        TreasureGroup treasureGroup;
        ValueReferenceGroup vrg;
        Data baseData;

        internal TreasureObject(TreasureGroup treasureGroup, int subid, Data baseData) {
            this.treasureGroup = treasureGroup;
            this.SubID = subid;

            // Check if index is too high
            if (ID >= Project.NumTreasures)
                throw new InvalidTreasureException(string.Format("ID {0:X2} for treasure was too high!", ID));

            this.baseData = baseData;
            GenerateValueReferenceGroup();
        }


        // Properties

        public ValueReferenceGroup ValueReferenceGroup {
            get { return vrg; }
        }

        public int Graphics {
            get { return vrg.GetIntValue("Graphics"); }
        }

        public int ID {
            get { return treasureGroup.Index; }
        }
        public int SubID {
            get; private set;
        }


        Project Project { get { return treasureGroup.Project; } }


        // Private methods

        void GenerateValueReferenceGroup() {
            vrg = new ValueReferenceGroup(new ValueReference[] {
                new DataValueReference(baseData,
                        index: 0,
                        name: "Spawn Mode",
                        type: DataValueType.ByteBits,
                        startBit: 4,
                        endBit: 6,
                        constantsMappingString: "TreasureSpawnModeMapping"),
                new DataValueReference(baseData,
                        index: 0,
                        name: "Set 'Item Obtained' Flag",
                        type: DataValueType.ByteBit,
                        startBit: 3,
                        tooltip: "Sets a flag indicating that an item has been received in this room. In the case of chests, this will make the chest \"opened\" when you revisit the room. (Flag is named \"ROOMFLAG_ITEM\" in the disassembly.)"),
                new DataValueReference(baseData,
                        index: 0,
                        name: "Grab Mode",
                        type: DataValueType.ByteBits,
                        startBit: 0,
                        endBit: 2,
                        constantsMappingString: "TreasureGrabModeMapping"),
                new DataValueReference(baseData.NextData,
                        index: 0,
                        name: "Parameter",
                        type: DataValueType.Byte),
                new DataValueReference(baseData.NextData.NextData,
                        index: 0,
                        name: "Text Index",
                        type: DataValueType.Byte,
                        tooltip: "Will show text \"TX_00XX\" when you open the chest."),
                new DataValueReference(baseData.NextData.NextData.NextData,
                        index: 0,
                        name: "Graphics",
                        type: DataValueType.Byte),
            });
        }
    }
}
