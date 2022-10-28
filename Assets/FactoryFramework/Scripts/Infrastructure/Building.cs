using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FactoryFramework
{
    public class Building : LogisticComponent
    {
        public UnityEvent<Building> OnBuildingDestroyed;

        public ConveyorSocket[] inputSockets;
        public ConveyorSocket[] outputSockets;

        private void Update()
        {
            this.ProcessLoop();
        }

        protected Recipe[] GetAllRecipes()
        {
            return Resources.LoadAll<Recipe>("");
        }

        private void OnDestroy()
        {
            OnBuildingDestroyed?.Invoke(this);
        }

        public ConveyorSocket GetInputSocketByIndex(int index)
        {
            if (index >= inputSockets.Length)
                return null;

            return inputSockets[index];
        }

        public ConveyorSocket GetOutputSocketByIndex(int index)
        {
            if (index >= outputSockets.Length)
                return null;

            return outputSockets[index];
        }

        public int GetInputIndexBySocket(ConveyorSocket cs)
        {
            for (int i = 0; i < inputSockets.Length; i++)
                if (cs == inputSockets[i])
                    return i;

            return -1;
        }

        public int GetOutputIndexBySocket(ConveyorSocket cs)
        {
            for (int i = 0; i < outputSockets.Length; i++)
                if (cs == outputSockets[i])
                    return i;

            return -1;
        }

    }
}