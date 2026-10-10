using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Day_01_G03;

namespace LINQ01
{
    public class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("================ LINQ - Restriction Operators ================");

            var outOfStockProducts = ListGenerator.ProductsList.Where(p => p.UnitsInStock == 0);
            Console.WriteLine("\n1. Products out of stock:");
            foreach (var p in outOfStockProducts)
            {
                Console.WriteLine(p);
            }

            var inStockCostMoreThan3 = ListGenerator.ProductsList.Where(p => p.UnitsInStock > 0 && p.UnitPrice > 3.00m);
            Console.WriteLine("\n2. Products in stock and cost more than 3.00:");
            foreach (var p in inStockCostMoreThan3)
            {
                Console.WriteLine(p);
            }

            string[] digitsArr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var shorterDigits = digitsArr.Where((name, index) => name.Length < index);
            Console.WriteLine("\n3. Digits whose name is shorter than their value:");
            foreach (var d in shorterDigits)
            {
                Console.WriteLine(d);
            }


            Console.WriteLine("\n================ LINQ - Element Operators ================");

            var firstOutOfStock = ListGenerator.ProductsList.FirstOrDefault(p => p.UnitsInStock == 0);
            Console.WriteLine("\n1. First Product out of Stock:");
            Console.WriteLine(firstOutOfStock?.ToString() ?? "None");

            var firstExpensive = ListGenerator.ProductsList.FirstOrDefault(p => p.UnitPrice > 1000m);
            Console.WriteLine("\n2. First product whose Price > 1000:");
            Console.WriteLine(firstExpensive != null ? firstExpensive.ToString() : "null");

            int[] numbersArr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var secondGreaterThanFive = numbersArr.Where(n => n > 5).Skip(1).FirstOrDefault();
            Console.WriteLine("\n3. Second number greater than 5:");
            Console.WriteLine(secondGreaterThanFive);


            Console.WriteLine("\n================ LINQ - Aggregate Operators ================");

            int oddCount = numbersArr.Count(n => n % 2 != 0);
            Console.WriteLine("\n1. Number of odd numbers in array:");
            Console.WriteLine(oddCount);

            var customersOrderCount = ListGenerator.CustomersList.Select(c => new
            {
                c.CustomerID,
                c.CustomerName,
                OrderCount = c.Orders.Length
            });
            Console.WriteLine("\n2. Customers and their order count:");
            foreach (var c in customersOrderCount)
            {
                Console.WriteLine($"{c.CustomerID} ({c.CustomerName}): {c.OrderCount} orders");
            }

            var categoryProductCounts = ListGenerator.ProductsList.GroupBy(p => p.Category).Select(g => new
            {
                Category = g.Key,
                ProductCount = g.Count()
            });
            Console.WriteLine("\n3. Categories and product counts:");
            foreach (var cat in categoryProductCounts)
            {
                Console.WriteLine($"{cat.Category}: {cat.ProductCount} products");
            }

            int totalSum = numbersArr.Sum();
            Console.WriteLine("\n4. Total of numbers in array:");
            Console.WriteLine(totalSum);

            string dictionaryPath = File.Exists("dictionary_english.txt")
                ? "dictionary_english.txt"
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionary_english.txt");

            if (File.Exists(dictionaryPath))
            {
                string[] dictionaryWords = File.ReadAllLines(dictionaryPath);

                long totalCharacters = dictionaryWords.Sum(w => (long)w.Length);
                Console.WriteLine("\n5. Total characters in dictionary_english.txt:");
                Console.WriteLine(totalCharacters);

                int shortestWordLength = dictionaryWords.Min(w => w.Length);
                Console.WriteLine("\n6. Length of shortest word:");
                Console.WriteLine(shortestWordLength);

                int longestWordLength = dictionaryWords.Max(w => w.Length);
                Console.WriteLine("\n7. Length of longest word:");
                Console.WriteLine(longestWordLength);

                double avgWordLength = dictionaryWords.Average(w => w.Length);
                Console.WriteLine("\n8. Average word length:");
                Console.WriteLine(avgWordLength);
            }


            Console.WriteLine("\n================ LINQ - Ordering Operators ================");

            var productsByName = ListGenerator.ProductsList.OrderBy(p => p.ProductName);
            Console.WriteLine("\n1. Products sorted by name:");
            foreach (var p in productsByName)
            {
                Console.WriteLine(p.ProductName);
            }

            string[] mixedWords = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
            var sortedWithCustomComparer = mixedWords.OrderBy(w => w, new CaseInsensitiveComparer());
            Console.WriteLine("\n2. Case-insensitive sort with custom comparer:");
            foreach (var w in sortedWithCustomComparer)
            {
                Console.WriteLine(w);
            }

            var productsByStockDesc = ListGenerator.ProductsList.OrderByDescending(p => p.UnitsInStock);
            Console.WriteLine("\n3. Products sorted by stock (highest to lowest):");
            foreach (var p in productsByStockDesc)
            {
                Console.WriteLine($"{p.ProductName}: {p.UnitsInStock}");
            }

            var digitsByLengthThenName = digitsArr.OrderBy(d => d.Length).ThenBy(d => d);
            Console.WriteLine("\n4. Digits sorted by length then alphabetically:");
            foreach (var d in digitsByLengthThenName)
            {
                Console.WriteLine(d);
            }

            var wordsByLengthThenCaseInsensitive = mixedWords.OrderBy(w => w.Length).ThenBy(w => w, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\n5. Sort by length then case-insensitive:");
            foreach (var w in wordsByLengthThenCaseInsensitive)
            {
                Console.WriteLine(w);
            }

            var productsByCategoryThenPrice = ListGenerator.ProductsList
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.UnitPrice);
            Console.WriteLine("\n6. Products sorted by category then unit price descending:");
            foreach (var p in productsByCategoryThenPrice)
            {
                Console.WriteLine($"{p.Category} - {p.ProductName}: {p.UnitPrice:C}");
            }

            var wordsByLengthThenCaseInsensitiveDesc = mixedWords
                .OrderBy(w => w.Length)
                .ThenByDescending(w => w, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\n7. Sort by length then case-insensitive descending:");
            foreach (var w in wordsByLengthThenCaseInsensitiveDesc)
            {
                Console.WriteLine(w);
            }

            var reversedDigitsWithI = digitsArr
                .Where(d => d.Length > 1 && d[1] == 'i')
                .Reverse();
            Console.WriteLine("\n8. Digits whose second letter is 'i' reversed:");
            foreach (var d in reversedDigitsWithI)
            {
                Console.WriteLine(d);
            }


            Console.WriteLine("\n================ LINQ - Transformation Operators ================");

            var productNames = ListGenerator.ProductsList.Select(p => p.ProductName);
            Console.WriteLine("\n1. Product names:");
            foreach (var name in productNames)
            {
                Console.WriteLine(name);
            }

            string[] fruitWords = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            var upperLowerWords = fruitWords.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() });
            Console.WriteLine("\n2. Uppercase and lowercase versions:");
            foreach (var item in upperLowerWords)
            {
                Console.WriteLine($"Upper: {item.Upper}, Lower: {item.Lower}");
            }

            var productRenamedPrice = ListGenerator.ProductsList.Select(p => new
            {
                p.ProductID,
                p.ProductName,
                Price = p.UnitPrice,
                p.Category
            });
            Console.WriteLine("\n3. Products with Price property:");
            foreach (var item in productRenamedPrice)
            {
                Console.WriteLine($"{item.ProductID}: {item.ProductName} ({item.Category}) - Price: {item.Price:C}");
            }

            var matchesPosition = numbersArr.Select((num, index) => new
            {
                Number = num,
                InPlace = (num == index)
            });
            Console.WriteLine("\n4. Number matches its position:");
            foreach (var item in matchesPosition)
            {
                Console.WriteLine($"{item.Number}: {item.InPlace}");
            }

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            var pairs = from a in numbersA
                        from b in numbersB
                        where a < b
                        select new { a, b };
            Console.WriteLine("\n5. Pairs where numbersA < numbersB:");
            foreach (var pair in pairs)
            {
                Console.WriteLine($"{pair.a} is less than {pair.b}");
            }

            var ordersLessThan500 = ListGenerator.CustomersList
                .SelectMany(c => c.Orders)
                .Where(o => o.Total < 500.00m);
            Console.WriteLine("\n6. Orders where total < 500.00:");
            foreach (var order in ordersLessThan500)
            {
                Console.WriteLine(order);
            }

            var ordersIn1998OrLater = ListGenerator.CustomersList
                .SelectMany(c => c.Orders)
                .Where(o => o.OrderDate.Year >= 1998);
            Console.WriteLine("\n7. Orders made in 1998 or later:");
            foreach (var order in ordersIn1998OrLater)
            {
                Console.WriteLine(order);
            }
        }
    }
}
