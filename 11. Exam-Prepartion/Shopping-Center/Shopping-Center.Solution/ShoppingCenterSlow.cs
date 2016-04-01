namespace Shopping_Center
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class ShoppingCenterSlow
    {
        private const string ProductAdded = "Product added";
        private const string XProductsDeleted = " products deleted";
        private const string NoProductsFound = "No products found";
        private const string IncorrectCommand = "Incorrect command";

        private readonly MultiDictionary<string, Product> _productsByName =
            new MultiDictionary<string, Product>(true);

        private readonly MultiDictionary<string, Product> _productsByProducer =
            new MultiDictionary<string, Product>(true);

        private readonly MultiDictionary<string, Product> _productsByNameAndProducer =
            new MultiDictionary<string, Product>(true);

        private readonly OrderedMultiDictionary<decimal, Product> _productsByPriceRange =
            new OrderedMultiDictionary<decimal, Product>(true);

        private string AddProduct(string name, string price, string producer)
        {
            var product = new Product
            {
                Name = name,
                Price = decimal.Parse(price),
                Producer = producer
            };

            this._productsByName.Add(name, product);
            this._productsByProducer.Add(producer, product);
            var nameAndProducer = Combine(name, producer);
            this._productsByNameAndProducer.Add(nameAndProducer, product);
            this._productsByPriceRange.Add(product.Price, product);

            return ProductAdded;
        }

        private string FindProductsByName(string name)
        {
            var productName = this._productsByName[name];

            return SortedProducts(productName);
        }

        private static string SortedProducts(ICollection<Product> products)
        {
            if (products.Any())
            {
                var sortedProduct = products.OrderBy(n => n);
                return string.Join(Environment.NewLine, sortedProduct);
            }

            return NoProductsFound;
        }

        private string FindProductsByProducer(string producer)
        {
            var productProducer = this._productsByProducer[producer];

            return SortedProducts(productProducer);
        }

        private string FindProductsByPriceRange(string from, string to)
        {
            var start = decimal.Parse(from);
            var end = decimal.Parse(to);

            var range = this._productsByPriceRange.Range(start, true, end, true).Values;

            return SortedProducts(range);
        }

        private string DeleteProductsByNameAndProducer(string name, string producer)
        {
            var nameAndProducer = name + ";" + producer;

            var productsToBeRemoved = this._productsByNameAndProducer[nameAndProducer];
            if (productsToBeRemoved.Any())
            {
                foreach (var product in productsToBeRemoved)
                {
                    this._productsByName.Remove(product.Name, product);
                    this._productsByPriceRange.Remove(product.Price, product);
                }

                var deletedProducts = this._productsByNameAndProducer[nameAndProducer].Count;

                this._productsByNameAndProducer.Remove(nameAndProducer);

                return deletedProducts + XProductsDeleted;
            }

            return NoProductsFound;
        }

        private string DeleteProductsByProducer(string producer)
        {
            var productsToBeRemoved = this._productsByProducer[producer];
            if (productsToBeRemoved.Any())
            {
                foreach (var product in productsToBeRemoved)
                {
                    this._productsByName.Remove(product.Name, product);
                    this._productsByPriceRange.Remove(product.Price, product);
                }

                var deletedProducts = this._productsByProducer[producer].Count;

                this._productsByProducer.Remove(producer);

                return deletedProducts + XProductsDeleted;
            }

            return NoProductsFound;
        }

        private static string Combine(string name, string producer)
        {
            var key = name + ";" + producer;

            return key;
        }

        public string ProcessCommand(string command)
        {
            var indexOfFirstSpace = command.IndexOf(' ');
            var method = command.Substring(0, indexOfFirstSpace);
            var parameterValues = command.Substring(indexOfFirstSpace + 1);
            var parameters = parameterValues.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            switch (method)
            {
                case "AddProduct":
                    return this.AddProduct(parameters[0], parameters[1], parameters[2]);
                case "DeleteProducts":
                    if (parameters.Length == 1)
                    {
                        return this.DeleteProductsByProducer(parameters[0]);
                    }
                    else
                    {
                        return this.DeleteProductsByNameAndProducer(parameters[0], parameters[1]);
                    }
                case "FindProductsByName":
                    return this.FindProductsByName(parameters[0]);
                case "FindProductsByPriceRange":
                    return this.FindProductsByPriceRange(parameters[0], parameters[1]);
                case "FindProductsByProducer":
                    return this.FindProductsByProducer(parameters[0]);
                default:
                    return IncorrectCommand;
            }
        }
    }
}
