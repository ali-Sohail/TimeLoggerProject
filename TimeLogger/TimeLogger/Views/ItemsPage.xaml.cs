﻿using System;
using System.ComponentModel;
using TimeLogger.Models;
using TimeLogger.ViewModels;
using Xamarin.Forms;

namespace TimeLogger.Views
{
  // Learn more about making custom code visible in the Xamarin.Forms previewer
  // by visiting https://aka.ms/xamarinforms-previewer
  [DesignTimeVisible(false)]
  public partial class ItemsPage : ContentPage
  {
    private readonly ItemsViewModel viewModel;

    public ItemsPage()
    {
      InitializeComponent();

      BindingContext = viewModel = new ItemsViewModel();
    }

    private async void OnItemSelected(object sender, EventArgs args)
    {
      BindableObject layout = (BindableObject)sender;
      Item item = (Item)layout.BindingContext;
      await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
    }

    private async void AddItem_Clicked(object sender, EventArgs e)
    {
      await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      if (viewModel.Items.Count == 0)
      {
        viewModel.IsBusy = true;
      }
    }
  }
}