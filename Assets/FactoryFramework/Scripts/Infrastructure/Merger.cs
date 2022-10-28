using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryFramework
{
    public class Merger : Building, IOutput, IInput
    {
        [SerializeField] private int inputIndex = 0; // modulo this number by inputSockets.Length

        public bool CanGiveOutput(Item filter = null)
        {
            if (outputSockets[0].conveyor == null || !outputSockets[0].conveyor.CanTakeInput(null)) return false;
            if (filter != null) Debug.LogWarning("Merger Does not Implement Item Filter Output");
            foreach (ConveyorSocket c in inputSockets)
                if (c.conveyor != null && c.conveyor.CanGiveOutput())
                    return true;
            return false;
        }
        // output type doesn't really matter
        public Item OutputType() { return inputSockets[inputIndex].conveyor.OutputType(); }
        public Item GiveOutput(Item filter = null)
        {
            if (filter != null) Debug.LogWarning("Merger Does not Implement Item Filter Output");
            Item result = null;
            if (inputSockets[inputIndex].conveyor==null || !inputSockets[inputIndex].conveyor.CanGiveOutput())
                GoToNextAvilable();
            result = inputSockets[inputIndex].conveyor.GiveOutput();
            GoToNextAvilable();
            return result;
        }

        void GoToNextAvilable()
        {
            for (int a = 0; a < inputSockets.Length; a++)
            {
                // loop through until we find the inputSockets ready for output
                inputIndex = (inputIndex + 1) % inputSockets.Length;
                if (inputSockets[inputIndex].conveyor != null && inputSockets[inputIndex].conveyor.CanGiveOutput())
                    break;
            }
        }

        

        public override void ProcessLoop()
        {
            base.ProcessLoop();

            if (!CanTakeInput(null) || !CanGiveOutput(null)) return;

            GoToNextAvilable();
            var i = inputSockets[inputIndex];
            Conveyor inputConveyor = i.conveyor;
            Conveyor outputConveyor = outputSockets[0].conveyor;

            outputConveyor.TakeInput(inputConveyor.GiveOutput());
        }

        private void OnDrawGizmos()
        {
            // doesnt matter item type
            if (CanGiveOutput())
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

        public void TakeInput(Item item)
        {
            outputSockets[0].conveyor.TakeInput(item);
        }

        public bool CanTakeInput(Item item)
        {
            return outputSockets[0].conveyor != null && outputSockets[0].conveyor.CanTakeInput(item);
        }
    }
}