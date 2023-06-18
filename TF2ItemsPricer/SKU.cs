using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TF2ItemsPricer
{
    /// <summary>
    /// C# version of https://github.com/Nicklason/node-tf2-sku
    /// </summary>
    public class SKU : IEquatable<SKU>
    {
        public ulong Defindex;
        public int Quality = 6;
        public bool craftable = true;
        public bool australium;
        public bool festive;
        public int quality2;
        public int killstreak;
        public int effect;
        public int paintkit;
        public int wear;
        public int target;
        public int craftnumber;
        public int crateseries;
        public int output;
        public int outputQuality;

        public SKU() { }

        public SKU(string sku)
        {
            var parsed = Parse(sku);

            Defindex = parsed.Defindex;
            Quality = parsed.Quality;
            craftable = parsed.craftable;
            australium = parsed.australium;
            festive = parsed.festive;
            quality2 = parsed.quality2;
            killstreak = parsed.killstreak;
            effect = parsed.effect;
            paintkit = parsed.paintkit;
            wear = parsed.wear;
            target = parsed.target;
            craftnumber = parsed.craftnumber;
            crateseries = parsed.crateseries;
            output = parsed.output;
            outputQuality = parsed.outputQuality;
        }

        public SKU(ulong defindex, int quality, bool craftable = true, bool australium = false, bool festive = false, int quality2 = 0, int killstreak = 0, int effect = 0, int paintkit = 0, int wear = 0, int target = 0, int craftnumber = 0, int crateseries = 0, int output = 0, int outputQuality = 0)
        {
            Defindex = defindex;
            Quality = quality;
            this.craftable = craftable;
            this.australium = australium;
            this.festive = festive;
            this.quality2 = quality2;
            this.killstreak = killstreak;
            this.effect = effect;
            this.paintkit = paintkit;
            this.wear = wear;
            this.target = target;
            this.craftnumber = craftnumber;
            this.crateseries = crateseries;
            this.output = output;
            this.outputQuality = outputQuality;
        }




        public static SKU Parse(string sku)
        {
            SKU res = new SKU();
            var parts = sku.Split(";");

            if (parts.Length > 0)
            {
                if (!ulong.TryParse(parts[0], out res.Defindex)) return null;

            }

            if (parts.Length > 1)
            {
                if (!int.TryParse(parts[1], out res.Quality)) return null;

            }

            for (int i = 2; i < parts.Length; i++)
            {
                var attribute = parts[i].Replace("-", "").ToLower();

                if (attribute == "uncraftable")
                {
                    res.craftable = false;
                }
                else if (attribute == "australium")
                {
                    res.australium = true;
                }
                else if (attribute == "festive")
                {
                    res.festive = true;
                }
                else if (attribute == "strange")
                {
                    res.quality2 = 11;
                }
                else if (attribute.StartsWith("kt"))
                {
                    if (!int.TryParse(attribute.Substring(2), out res.killstreak)) res.killstreak = 0;
                }
                else if (attribute.StartsWith("u"))
                {
                    if (!int.TryParse(attribute.Substring(1), out res.effect)) res.effect = 0;
                }
                else if (attribute.StartsWith("pk"))
                {
                    res.paintkit = parseInt(attribute.Substring(2));
                }
                else if (attribute.StartsWith("w"))
                {
                    res.wear = parseInt(attribute.Substring(1));
                }
                else if (attribute.StartsWith("td"))
                {
                    res.target = parseInt(attribute.Substring(2));
                }
                else if (attribute.StartsWith("n"))
                {
                    res.craftnumber = parseInt(attribute.Substring(1));
                }
                else if (attribute.StartsWith("c"))
                {
                    res.crateseries = parseInt(attribute.Substring(1));
                }
                else if (attribute.StartsWith("od"))
                {
                    res.output = parseInt(attribute.Substring(2));
                }
                else if (attribute.StartsWith("oq"))
                {
                    res.outputQuality = parseInt(attribute.Substring(2));
                }
            }

            return res;
        }

        public override string ToString()
        {
            return $"{Defindex};{Quality}" +
                $"{(effect != 0 ? $";u{effect}" : "")}" +
                $"{(australium ? ";australium" : "")}" +
                $"{(!craftable ? ";uncraftable" : "")}" +
                $"{(wear != 0 ? $";w{wear}" : "")}" +
                $"{(paintkit != 0 ? $";pk{paintkit}" : "")}" +
                $"{(quality2 == 11 ? ";strange" : "")}" +
                $"{(killstreak > 0 ? $";kt-{killstreak}" : "")}" +
                $"{(target != 0 ? $";td-{target}" : "")}" +
                $"{(festive ? ";festive" : "")}" +
                $"{(craftnumber != 0 ? $";n{craftnumber}" : "")}" +
                $"{(crateseries != 0 ? $";c{crateseries}" : "")}" +
                $"{(output != 0 ? $";od-{output}" : "")}" +
                $"{(outputQuality != 0 ? $";oq-{outputQuality}" : "")}";


        }

        private static int parseInt(string str)
        {
            int res = 0;
            if (int.TryParse(str, out res)) return res;
            return 0;
        }

        public bool Equals(SKU? other)
        {
            if (other == null) return false;
            return Defindex == other.Defindex &&
                    Quality == other.Quality &&
                    craftable == other.craftable &&
                    australium == other.australium &&
                    festive == other.festive &&
                    quality2 == other.quality2 &&
                    killstreak == other.killstreak &&
                    effect == other.effect &&
                    paintkit == other.paintkit &&
                    wear == other.wear &&
                    target == other.target &&
                    craftnumber == other.craftnumber &&
                    crateseries == other.crateseries &&
                    output == other.output &&
                    outputQuality == other.outputQuality;
        }
    }
}
