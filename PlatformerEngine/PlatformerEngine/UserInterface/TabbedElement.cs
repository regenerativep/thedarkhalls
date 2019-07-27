using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// an element that contains tabs and content for each tab
    /// </summary>
    public class TabbedElement : GroupElement
    {
        /// <summary>
        /// a list of the tab buttons to switch the tab
        /// </summary>
        public HorizontalListElement TabButtonList;
        /// <summary>
        /// information on each of the tabs
        /// </summary>
        public Dictionary<string, KeyValuePair<ButtonElement, UIElement>> Tabs;
        /// <summary>
        /// container for the current tab content
        /// </summary>
        public GroupElement CurrentTabContainer; //TODO: make an actual container element instead of using a group element
        /// <summary>
        /// creates a new tabbed element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">element name</param>
        /// <param name="buttonSectionHeight">height of the tab buttons</param>
        public TabbedElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, int buttonSectionHeight) : base(uiManager, position, size, layer, name)
        {
            TabButtonList = new HorizontalListElement(UIManager, new Vector2(0, 0), new Vector2(Size.X, buttonSectionHeight), layer, Name + "_buttonlist");
            Tabs = new Dictionary<string, KeyValuePair<ButtonElement, UIElement>>();
            CurrentTabContainer = new GroupElement(UIManager, new Vector2(0, buttonSectionHeight), new Vector2(Size.X, Size.Y - buttonSectionHeight), layer, Name + "_tabcontainer");
            Elements.Add(TabButtonList);
            Elements.Add(CurrentTabContainer);
        }
        /// <summary>
        /// adds a tab
        /// </summary>
        /// <param name="name">name of the tab</param>
        /// <param name="tabContent">content for the tab</param>
        /// <param name="buttonWidth">width of the button of the tab</param>
        public void AddTab(string name, GroupElement tabContent, int buttonWidth = 64)
        {
            ButtonElement tabButton = new ButtonElement(UIManager, new Vector2(0, 0), new Vector2(buttonWidth, TabButtonList.Size.Y), Layer + 0.01f, Name + "_" + name + "_button", name);
            tabButton.Click = () =>
            {
                SetTab(name);
            };
            Tabs.Add(name, new KeyValuePair<ButtonElement, UIElement>(tabButton, tabContent));
            TabButtonList.AddItem(tabButton);
        }
        /// <summary>
        /// sets the current tab
        /// </summary>
        /// <param name="name">name of the tab to set to</param>
        public void SetTab(string name)
        {
            UIElement content = GetTabContent(name);
            CurrentTabContainer.Elements.Clear();
            if (content != null)
            {
                CurrentTabContainer.Elements.Add(content);
            }
        }
        /// <summary>
        /// gets the content of a tab
        /// </summary>
        /// <param name="name">the tab's name</param>
        /// <returns>the content of the tab under the given name if found</returns>
        public UIElement GetTabContent(string name)
        {
            if(Tabs.ContainsKey(name))
            {
                return Tabs[name].Value;
            }
            return null;
        }
        public override void Destroy(bool hardDestroy = false)
        {
            if (CurrentTabContainer != null)
            {
                CurrentTabContainer.Destroy(hardDestroy);
            }
            TabButtonList.Destroy(hardDestroy);
            base.Destroy();
        }
    }
}
