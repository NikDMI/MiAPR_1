using System;
using MiAPR_1.Models;
using MiAPR_1.CustomControls;
using System.Collections.Generic;
using System.Windows.Media;

namespace MiAPR_1.ViewModels
{
    public class ViewModel
    {
        public ViewModel()
        {

        }


        public void GenerateVectors(AlgorithmInfo algorithmInfo, VectorField fieldBefore, VectorField fieldAfter)
        {
            List<VectorModel> generatedVectors = new List<VectorModel>(algorithmInfo.ObjectsCount);
            List<VectorModel> coresVectors = new List<VectorModel>();//ядра
            for (int i = 0; i < algorithmInfo.ObjectsCount; ++i)
            {
                generatedVectors.Add(new VectorModel() { X = _random.Next(0, 10000), Y = _random.Next(0, 10000), Brush = Brushes.Aqua });
            }
            List<int> generatedCoresIndexes = new List<int>();
            for(int i = 0; i < algorithmInfo.ClassCount; ++i)
            {
                int coreIndex = 0;
                while (true)
                {
                    coreIndex = _random.Next(0, algorithmInfo.ObjectsCount - 1);
                    if (generatedCoresIndexes.Contains(coreIndex)) continue;
                    generatedCoresIndexes.Add(coreIndex);
                    break;
                }
                coresVectors.Add(generatedVectors[coreIndex]);
                generatedVectors[coreIndex].Brush = new SolidColorBrush() { Color = Color.FromArgb(
                    255, (byte)_random.Next(30,200), (byte)_random.Next(30, 200), (byte)_random.Next(30, 200))};
            }
            CreateVectorLayouts(coresVectors, generatedVectors);
            List<VectorModel> computedVectors = CopyVectorList(generatedVectors);//vectors after algotithm
            List<VectorModel> coreComutedVectors = new List<VectorModel>();
            foreach (int index in generatedCoresIndexes)
            {
                coreComutedVectors.Add(computedVectors[index]);
            }
            PerformKAverageAlgorithm(computedVectors, coreComutedVectors);
            fieldBefore.SetVectors(generatedVectors);
            fieldAfter.SetVectors(computedVectors);
        }

        private double GetVectorsDistance(VectorModel v1, VectorModel v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }


        //First step of algorithm
        private void CreateVectorLayouts(List<VectorModel> cores, List<VectorModel> allVectors)
        {
            foreach(var vector in allVectors)
            {

                if (cores.Contains(vector)) continue;
                vector.Brush = cores[0].Brush;
                double currentDistance = GetVectorsDistance(vector, cores[0]);
                for (int i = 1; i < cores.Count; ++i)
                {
                    double nextDistance = GetVectorsDistance(vector, cores[i]);
                    if (nextDistance < currentDistance)
                    {
                        currentDistance = nextDistance;
                        vector.Brush = cores[i].Brush;
                    }
                }
            }
        }


        private void PerformKAverageAlgorithm(List<VectorModel> vectors, List<VectorModel> coreVectors)
        {
            List<VectorModel> countedCores = new List<VectorModel>(coreVectors);   //new cores
            while (true)
            {
                List<VectorModel> invalidCores = new List<VectorModel>();
                for (int i = 0; i < countedCores.Count; ++i)
                {
                    var nextPerfectCore = FindNextCore(countedCores[i], vectors);
                    if (nextPerfectCore != countedCores[i]) //set new core
                    {
                        countedCores[i] = nextPerfectCore;
                        invalidCores.Add(nextPerfectCore);
                    }
                }
                if (invalidCores.Count == 0)    //if all cores DIDN'T CAHNGED
                {
                    coreVectors.Clear(); coreVectors.AddRange(countedCores);
                    break;
                }
                //Change all invalid groups
                List<VectorModel> invalidVectors = new List<VectorModel>();
                foreach (var vector in vectors) //find invalid vectors
                {
                    foreach(var coreVector in countedCores)
                    {
                        if (coreVector.Brush == vector.Brush)
                        {
                            invalidVectors.Add(vector);
                            break;
                        }
                    }
                }
                CreateVectorLayouts(countedCores, invalidVectors);
            }
        }


        //Try to find next core in vector's regions
        private VectorModel FindNextCore(VectorModel core, List<VectorModel> vectors)
        {
            double currentPersistance = double.MaxValue;
            VectorModel currentCore = null;
            foreach(var conditionCore in vectors)
            {
                if (conditionCore.Brush == core.Brush) //If vector belongs to the group
                {
                    //Check it's persistance
                    double persistance = 0;
                    foreach(var innerVector in vectors)
                    {
                        if (persistance > currentPersistance) break;
                        if (innerVector.Brush == core.Brush)
                        {
                            persistance += Math.Pow(GetVectorsDistance(innerVector, conditionCore), 2);
                        }
                    }
                    if (persistance < currentPersistance)//If this core is more accurate
                    {
                        currentPersistance = persistance;
                        currentCore = conditionCore;
                    }
                }
            }
            return currentCore;
        }


        private List<VectorModel> CopyVectorList(List<VectorModel> vectors)
        {
            List<VectorModel> vectorModels = new List<VectorModel>();
            vectors.ForEach(vector => vectorModels.Add(new VectorModel()
            {
                X = vector.X,
                Y = vector.Y,
                Brush = vector.Brush
            }));
            return vectorModels;
        }

        private Random _random = new Random();
    }
}
