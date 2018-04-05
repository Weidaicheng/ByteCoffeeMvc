using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteCoffee.Utility.Data
{
    public abstract class Node
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string URL { get; set; }

        public string ClassName { get; set; }

        public bool IsLeaf { get; set; }

        public string Ico { get; set; }

        public Node()
        {
        }

        public Node(string name, string value, string url, string classname, string ico, bool isleaf = false)
        {
            this.Name = name;
            this.Value = value;
            this.URL = url;
            this.ClassName = classname;
            this.IsLeaf = isleaf;
            this.Ico = ico;
        }

        public abstract void Add(Node _node);

        public abstract void Remove(Node _node);

        public abstract string Display(bool IsShowRoot = true, string FatherPathName = "");
    }

    public class TreeNode : Node
    {
        public TreeNode()
            : base()
        { }

        public TreeNode(string name, string value, string url, string classname, string ico, bool isleaf = false)
            : base(name, value, url, classname, ico, isleaf)
        { }

        private List<Node> children = new List<Node>();

        public override void Add(Node _node)
        {
            if (_node != null && !children.Contains(_node))
            { children.Add(_node); }
        }

        public override void Remove(Node _node)
        {
            if (_node != null && children.Contains(_node))
            { children.Remove(_node); }
        }

        public override string Display(bool IsShowRoot = true, string FatherPathName = "")
        {
            if (FatherPathName.Length > 0)
            { FatherPathName += IsShowRoot ? ">" + this.Name : string.Empty; }
            else
            { FatherPathName = IsShowRoot ? this.Name : string.Empty; }

            StringBuilder container = new StringBuilder();
            if (!this.IsLeaf)
            {
                container.Append(IsShowRoot ? BuildCatalogNodeFront(this.Name, this.Ico) : string.Empty);
                foreach (Node item in children)
                {
                    if (item.IsLeaf)
                    { container.Append(BuildLeafNode(item.Name, item.Value, item.URL, item.Ico, FatherPathName)); }
                    else
                    { container.Append(item.Display(true, FatherPathName)); }
                }
                container.Append(IsShowRoot ? BuildCatalogNodeEnd() : string.Empty);
            }
            else
            { container.Append(BuildLeafNode(this.Name, this.Value, this.URL, this.Ico, FatherPathName)); }
            return container.ToString();
        }

        public virtual string BuildLeafNode(string name, string value, string url, string ico, string fathernodename)
        {
            return string.Format("<li><a class='J_menuItem' data-index='{3}' href='{0}' data-tabtitle='{1},{2}' title='{2}'>{4}<span>{2}</span></a></li>",
                url, fathernodename, name, value, !string.IsNullOrEmpty(ico) ? "<i class='" + ico + "'></i>" : "");
        }

        public virtual string BuildCatalogNodeFront(string name, string ico)
        { return string.Format("<li><a href='###'><i class='{1}'></i><span class='nav-label'>{0}</span><span class='fa arrow'></span></a><ul class='nav nav-second-level collapse' aria-expanded='false'>", name, !string.IsNullOrEmpty(ico) ? ico : "fa fa-server"); }

        public virtual string BuildCatalogNodeEnd()
        { return "</ul></li>"; }
    }

    /// <summary>
    /// 角色树节点(用户创建目录菜单功能树)
    /// </summary>
    public class RoleTreeNode
    {
        public int Id { get; set; }

        public int FatherId { get; set; }

        public string Title { get; set; }

        public int NodeType { get; set; }

        public int IsEnabled { get; set; }

        public int Depth { get; set; }

        public bool IsChecked { get; set; }

        private List<RoleTreeNode> _childitems = new List<RoleTreeNode>();

        public List<RoleTreeNode> ChildItems
        { get { return _childitems; } }

        public RoleTreeNode()
        {
        }

        public RoleTreeNode(int id, int fatherid, string title, int nodetype, int isenabled)
        {
            this.Id = id;
            this.FatherId = fatherid;
            this.Title = title;
            this.NodeType = nodetype;
            this.IsEnabled = isenabled;
        }

        public void Add(RoleTreeNode _node)
        {
            if (_node != null && !ChildItems.Contains(_node))
            {
                _node.Depth = this.Depth + 1;
                _childitems.Add(_node);
            }
        }

        public void Remove(RoleTreeNode _node)
        {
            if (_node != null && ChildItems.Contains(_node))
            { _childitems.Remove(_node); }
        }

        /// <summary>
        /// Displays the specified is show root.
        /// </summary>
        /// <param name="IsShowRoot">if set to <c>true</c> [is show root].</param>
        /// <param name="CheckdList">The checkd list.</param>
        /// <returns></returns>
        public string Display(List<int> CheckdList = null)
        {
            StringBuilder container = new StringBuilder();
            if (this.NodeType != 3)
            {
                //display self catalog
                if (this.NodeType == 2)
                {
                    //menu
                    if (CheckdList != null && CheckdList.Contains(this.Id))
                    { this.IsChecked = true; }
                    container.Append(BuildMenuNodeFront(this.Id, this.Title, this.Depth, this.IsChecked));
                }
                else
                { container.Append(this.Id != 0 ? BuildCatalogNodeFront(this.Id, this.Title, this.Depth) : string.Empty); }

                //display self leafs
                var selfleafs = from c in _childitems where c.NodeType == 3 select c;
                container.Append(this.Id != 0 ? BuildLeafFront() : string.Empty);
                foreach (RoleTreeNode leaf in selfleafs)
                {
                    if (CheckdList != null && CheckdList.Contains(leaf.Id))
                    { leaf.IsChecked = true; }
                    container.Append(BuildLeafNodes(leaf.Id, leaf.Title, leaf.IsChecked));
                }
                container.Append(this.Id != 0 ? BuildLeafEnd() : string.Empty);
                //end tag
                BuildCatalogAndMenuNodeEnd();

                foreach (RoleTreeNode item in _childitems)
                {
                    if (item.NodeType != 3)
                    { container.Append(item.Display(CheckdList)); }
                }
            }
            return container.ToString();
        }

        /// <summary>
        /// Builds the leaf front.
        /// </summary>
        /// <returns></returns>
        public virtual string BuildLeafFront()
        { return "<td>"; }

        /// <summary>
        /// Builds the leaf end.
        /// </summary>
        /// <returns></returns>
        public virtual string BuildLeafEnd()
        { return "</td>"; }

        /// <summary>
        /// Builds the leaf nodes.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="ischecked">if set to <c>true</c> [ischecked].</param>
        /// <returns></returns>
        public virtual string BuildLeafNodes(int id, string name, bool ischecked)
        {
            string clientid = id.ToString();
            string checkedattr = ischecked ? "checked='checked'" : string.Empty;
            return string.Format("<input id='{0}' class='leafs' type='checkbox' {1} /><label for='{0}'>{2}</label>", clientid, checkedattr, name);
        }

        /// <summary>
        /// Builds the catalog node front.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        public virtual string BuildMenuNodeFront(int id, string name, int depth, bool ischecked)
        {
            string clientid = id.ToString();
            string checkedattr = ischecked ? "checked='checked'" : string.Empty;
            return string.Format("<tr><td>{0}<input id='{1}' class='menus' type='checkbox' {2} /><label for='{1}'>{3}</label></td>", BuildBlockByCount(depth),
                                                                                                                                        clientid,
                                                                                                                                        checkedattr,
                                                                                                                                        name);
        }

        /// <summary>
        /// Builds the catalog node front.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        public virtual string BuildCatalogNodeFront(int id, string name, int depth)
        {
            return string.Format("<tr><td id='{0}'>{1}{2}</td>", id.ToString(), BuildBlockByCount(depth), name);
        }

        /// <summary>
        /// Builds the catalog node end.
        /// </summary>
        /// <returns></returns>
        public virtual string BuildCatalogAndMenuNodeEnd()
        { return "</tr>"; }

        /// <summary>
        /// Builds the block by count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public virtual string BuildBlockByCount(int count)
        {
            string returnval = string.Empty;
            for (int i = 0; i < count; i++)
            { returnval += "<span></span>"; }
            return returnval;
        }
    }
}