//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace demo_ef
{
    using System;
    using System.Collections.Generic;
    
    public partial class GMS_WorkOrderPoints
    {
        public int ID { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public int WorkOrder_ID { get; set; }
        public bool HasSend { get; set; }
    }
}