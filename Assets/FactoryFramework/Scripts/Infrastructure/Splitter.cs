using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryFramework
{
    public class Splitter : Building, IInput, IOutput
    {
        [SerializeField] private int outputIndex = 0; // modulo this number by outputSockets.Length

        public bool CanTakeInput(Item item)
        {
            foreach (ConveyorSocket c in outputSockets)
                if (c.conveyor != null && c.conveyor.CanTakeInput(item))
                    return true;
            return false;
        }
        public void TakeInput(Item item)
        {
            if (outputSockets[outputIndex].conveyor == null || !outputSockets[outputIndex].conveyor.CanTakeInput(item))
            {
                GoToNextAvilable(item);
            }
            outputSockets[outputIndex].conveyor.TakeInput(item);
            GoToNextAvilable(item);
            return;
        }

        public override void ProcessLoop()
        {
            base.ProcessLoop();

            if (!CanGiveOutput()) return;

            GoToNextAvilable(null);
            var o = outputSockets[outputIndex];
            Conveyor outputConveyor = o.conveyor;

            outputConveyor.TakeInput(GiveOutput(null));

        }

        void GoToNextAvilable(Item item)
        {
            for (int a = 0; a < outputSockets.Length; a++)
            {
                // loop through until we find the output ready to take input
                outputIndex = (outputIndex + 1) % outputSockets.Length;
                if (outputSockets[outputIndex].conveyor != null && outputSockets[outputIndex].conveyor.CanTakeInput(item))
                    return;
            }

        }

        private void OnDrawGizmos()
        {
            // doesnt matter item type
            if (CanTakeInput(null))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 1f);
        }

        public Item OutputType()
        {
            return inputSockets[0].conveyor.OutputType();
        }

        public Item GiveOutput(Item filter = null)
        {
            return inputSockets[0].conveyor.GiveOutput(filter);
        }

        public bool CanGiveOutput(Item filter = null)
        {
            // if no conveyors have room, return false
            if (!CanTakeInput(null)) {            
                return false; 
            }
            if (inputSockets[0].conveyor != null && inputSockets[0].conveyor.CanGiveOutput(null)) return true;

            return false;

        }
    }
}