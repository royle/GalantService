﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace GLTWarter.Pages.Order
{
    /// <summary>
    /// Interaction logic for CustomerServiceBookingOrder.xaml
    /// </summary>
    public partial class CustomerServiceBookingOrder : DetailsBase
    {
        public CustomerServiceBookingOrder(Galant.DataEntity.Paper paper):base(paper)
        {
            InitializeComponent();
            packet.PaperId = paper.PaperId;
            packet.PackageType = Galant.DataEntity.PackageState.New;
            this.packetageSelecter.DataContext = packet;
        }

        private Galant.DataEntity.Package packet = new Galant.DataEntity.Package(string.Empty);

        private void btnBookPaper_Click(object sender, RoutedEventArgs e)
        {
            this.buttonNext_Click(sender, e);
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            Galant.DataEntity.Paper paper = new Galant.DataEntity.Paper();
            paper.ContactA = new Galant.DataEntity.Entity();
            paper.Packages = new ObservableCollection<Galant.DataEntity.Package>();
            paper.Operation = BaseOperatorName.DataSave;
            this.DataContext = paper;
            this.dataCurrent = packet;
        }

        private void EntitySelector_Enter(object sender, GLTWarter.Controls.EntitySelectorEnterEventArgs e)
        {
            if (this.entitySelector.SelectedEntity != null)
            {
                (this.DataContext as Galant.DataEntity.Paper).ContactA = this.entitySelector.SelectedEntity;
            }
        }

        private void ProductSelector_Enter(object sender, GLTWarter.Controls.ProductSelectorEnterEventArgs e)
        {
            try
            {
                this.packet.Product = productSelector.SelectedProduct;
                this.packet.Amount = this.packet.Product.Amount * this.packet.Count;
            }
            catch
            { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (this.productSelector.SelectedProduct == null || string.IsNullOrEmpty(this.txtCount.Text))
            {
                Utils.PlaySound(Resource.soundMismatch);
                return;
            }
            if (this.packet.Product != null)
            {
                this.packet.Amount = this.packet.Product.Amount * this.packet.Count;
                (this.DataContext as Galant.DataEntity.Paper).Packages.Add(this.packet.Clone() as Galant.DataEntity.Package);
            }
        }
    }
}
