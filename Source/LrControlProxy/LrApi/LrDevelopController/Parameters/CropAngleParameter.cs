// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LrControlProxy.LrApi.LrDevelopController.Parameters
{
    public class CropAngleParameter : Parameter
    {
        public static readonly CropAngleParameter straightenAngle = new CropAngleParameter("straightenAngle", "Angle");

        public static readonly IList<CropAngleParameter> AllParameters =
            new ReadOnlyCollection<CropAngleParameter>(new List<CropAngleParameter>
            {
                straightenAngle
            });

        private CropAngleParameter(string name, string displayName) : base(name, displayName, typeof(int))
        {
        }
    }
}