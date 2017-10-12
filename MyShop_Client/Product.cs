using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClientOfMyShop
{
    class Product
    {
        public string mName;
        public int mAmount;
        public double mPrice;

        public Product()
        {
            Random r = new Random(GetHashCode());
            mName = "defaultProdName";
            mAmount = r.Next(1, 99);
            mPrice = r.NextDouble() * 10;
        }

        public void SaveProduct(BinaryWriter bw)
        {
            bw.Write(this.mName);
            bw.Write(this.mAmount);
            bw.Write(this.mPrice);
        }

        public void RestoreProduct(ref BinaryReader br, ref Product pr)
        {

            pr.mName = br.ReadString();
            pr.mAmount = br.ReadInt32();
            pr.mPrice = br.ReadDouble();

        }

        public void ShowProduct()
        {
            Console.WriteLine("name - " + this.mName + ',' + " amount - " + this.mAmount + ',' + " price - " + this.mPrice);
        }
    }
}

