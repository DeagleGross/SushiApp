using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SushiApp.Models
{
    public class ListViewFormer
    {
        /// <summary>
        /// returns the list of LISTVIEWITEM to be displayed on page
        /// </summary>
        /// <returns>The list for page.</returns>
        public List<ListViewItem> GetListForPage(string type)
        {
            // got list of items from mobile database
            List<Item> inDB = App.ItemDatabase.GetItemsOfType(type);

            List<ListViewItem> tmp = new List<ListViewItem>();

            foreach (Item item in inDB)
            {
                // here we need to convert 'item' object to 'listviewitem' object
                // and inside constructor there is a convertion 
                // bitmap --> xamarin.forms.image
                ListViewItem lst = new ListViewItem(item.Name, item.Price, item.Description, item.Pic);

                // adding creating instance to list of shown products
                tmp.Add(lst);
            }

            return tmp;
        }

        /// <summary>
        /// returns the ListView instance with all convertions made
        /// </summary>
        /// <returns>The list view instance.</returns>
        /// <param name="Items">Items.</param>
        public ListView CreateListViewInstance(List<ListViewItem> Items)
        {
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = Items,
                // Определяем формат отображения данных
                ItemTemplate = new DataTemplate(() =>
                {
                    // main layout with both image + text AND button
                    StackLayout MainLayout = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 8 };
                    // horizontal layout to align image to left, text to right
                    StackLayout Horizontal = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 6 };
                    // vertical layout for text to be one under another
                    StackLayout Vertical = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 6 };


                    // привязка к свойству Name
                    Label nameLabel = new Label
                    {
                        FontSize = 25,
                        TextColor = Color.Indigo,
                        FontAttributes = FontAttributes.Bold
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    // привязка к свойству Price
                    Label priceLabel = new Label
                    {
                        FontSize = 15,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.BlueViolet
                    };
                    Binding priceBinding = new Binding { Path = "Price", StringFormat = "{0} рублей" };
                    priceLabel.SetBinding(Label.TextProperty, priceBinding);

                    // привязка к свойству Description
                    Label descriptionLabel = new Label { FontAttributes = FontAttributes.Italic };
                    descriptionLabel.SetBinding(Label.TextProperty, "Description");

                    Image image = new Image { HeightRequest = 150, WidthRequest = 130 };
                    image.SetBinding(Image.SourceProperty, new Binding("ImageBytes", BindingMode.OneWay, new ByteArrayToImageConverter()));

                    Button buttonAddToCart = new Button
                    {
                        TextColor = Color.DarkRed,
                        Text = "Добавить в корзину",
                        FontAttributes = FontAttributes.Bold,
                        //BorderColor = Color.DarkOrchid,
                        //BorderWidth = 2,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    // event handler of clicking the propper button
                    buttonAddToCart.SetBinding(Button.CommandParameterProperty, "Name");
                    buttonAddToCart.SetBinding(Button.CommandProperty, "AddToCartCommand");

                    Vertical.Children.Add(nameLabel);
                    Vertical.Children.Add(priceLabel);
                    Vertical.Children.Add(descriptionLabel);

                    Horizontal.Children.Add(image);
                    Horizontal.Children.Add(Vertical);

                    MainLayout.Children.Add(Horizontal);
                    MainLayout.Children.Add(buttonAddToCart);

                    // создаем объект ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            //Children = { nameLabel, priceLabel, descriptionLabel, image }
                            Children = { MainLayout }
                        }
                    };
                })
            };

            return listView;
        }
    }
}
