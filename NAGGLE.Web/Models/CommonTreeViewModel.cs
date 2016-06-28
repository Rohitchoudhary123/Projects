using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class TreeViewControlModel
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string ParentId { get; set; }
        public bool isParent { get; set; }
        public bool IsSelectable { get; set; }
    }
}