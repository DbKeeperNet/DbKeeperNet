using System;

#if _PCL
namespace System
{
    public class SerializableAttribute : Attribute
    {
         
    }
}

namespace System.ComponentModel
{
    public class DesignerCategoryAttribute : Attribute
    {
        public DesignerCategoryAttribute(string category)
        {
        }
    }
}
#endif
