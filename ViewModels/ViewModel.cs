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
            fieldBefore.SetVectors(generatedVectors);
        }

        private double GetVectorsDistance(VectorModel v1, VectorModel v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }


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

        private Random _random = new Random();
    }
}
