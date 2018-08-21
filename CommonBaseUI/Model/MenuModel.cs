using System.Collections.Generic;

namespace CommonBaseUI.Model
{
    public class MenuResModel : ResponseModelBase
    {
        public MenuResModel()
        {
            this.menuList = new List<MenuModel>();
        }
        public List<MenuModel> menuList { get; set; }
    }

    public class MenuModel
    {
        public MenuModel()
        {
            this.subList = new List<MenuModel>();
        }

        public string id { get; set; }
        public string menuTitle { get; set; }
        public string iconName { get; set; }
        public string url { get; set; }

        public List<MenuModel> subList { get; set; }
    }
}
