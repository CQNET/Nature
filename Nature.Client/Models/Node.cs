using System;
using System.Collections.Generic;
using System.Text;

namespace Nature.Client.Models
{
    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string Status { get; set; }

        public Hall HallName { get; set; }
    }
    public enum Hall
    {
        见林厅,
        重庆厅,
        恐龙厅,
        地球厅,
        进化厅,
        环境厅
    }
}
