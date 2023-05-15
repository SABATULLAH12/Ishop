using Aspose.Slides;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSHOPNew.Models
{
    interface IBevaragesShopper
    {
        void Slide_1(ISlide slide, DataSet Ds);
        void Slide_2(ISlide slide, DataSet Ds);
        void Slide_3(ISlide slide, DataSet Ds);
        void Slide_4(ISlide slide, DataSet Ds);
        void Slide_5(ISlide slide, DataSet Ds);
        void Slide_6(ISlide slide, DataSet Ds);
        void Slide_7(ISlide slide, DataSet Ds);
    }
}
