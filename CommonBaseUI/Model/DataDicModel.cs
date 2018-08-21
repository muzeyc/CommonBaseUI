
using System.Collections.Generic;
namespace CommonBaseUI.Model
{
    public class DataDicResModel : ResponseModelBase
    {
        public DataDicResModel()
        {
            this.list = new List<DataDicModel>();
        }
        public List<DataDicModel> list { get; set; }
    }

    public class DataDicModel
    {
        public string val { get; set; }
        public string name { get; set; }
    }
}
