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
            for (int i = 0; i < algorithmInfo.ObjectsCount; ++i)
            {
                generatedVectors.Add(new VectorModel() { X = _random.Next(0, 10000), Y = _random.Next(0, 10000), Brush = Brushes.Aqua });
            }
            fieldBefore.SetVectors(generatedVectors);
        }

        private Random _random = new Random();
    }
}
