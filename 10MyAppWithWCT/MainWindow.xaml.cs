using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyAppWithWCT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyAppWithWCT
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public Product[]? Products { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.PopulateProducts();
        }

        private void PopulateProducts()
        {
            Products = new Product[]
            {
                new Product{ Id = 1, Name = "Roma 1kg", Category = "Detergente en polvo", Quantity = 5, Cost = 35, Price = 45},
                new Product{ Id = 2, Name = "Ariel Revitacolor 800g", Category = "Detergente en polvo", Quantity = 15, Cost = 39, Price = 49},
                new Product{ Id = 3, Name = "Coca Cola No Retornable 3L", Category = "Refrescos", Quantity = 16, Cost = 45, Price = 52},
                new Product{ Id = 4, Name = "Sprite 3L", Category = "Refrescos", Quantity = 8, Cost = 38, Price = 45},
                new Product{ Id = 5, Name = "Italpasta Fideo Cortado 200g", Category = "Sopas", Quantity = 12, Cost = 9, Price = 12},
                new Product{ Id = 6, Name = "Italpasta Cabello de Angel 200g", Category = "Sopas", Quantity = 3, Cost = 9, Price = 12},
                new Product{ Id = 7, Name = "Maravillas 1L", Category = "Aceites", Quantity = 8, Cost = 35, Price = 45},
                new Product{ Id = 8, Name = "Gran tradición 1L", Category = "Aceites", Quantity = 12, Cost = 35, Price = 42},
                new Product{ Id = 9, Name = "Alpura Clásica 1L", Category = "Lacteo", Quantity = 6, Cost = 25, Price = 32},
                new Product{ Id = 10, Name = "Nutri 1L", Category = "Lacteo", Quantity = 12, Cost = 18, Price = 22},
                new Product{ Id = 11, Name = "Regio Rinde + 4 Rollos (400h c/u)", Category = "Desechable", Quantity = 10, Cost = 25, Price = 32},
                new Product{ Id = 12, Name = "Vogue 4 Rollos (600h c/u)", Category = "Desechable", Quantity = 5, Cost = 35, Price = 42},
            };
        }
    }
}
