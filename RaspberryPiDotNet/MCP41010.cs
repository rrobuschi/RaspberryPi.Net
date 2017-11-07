using System;

// Original author: Mikey Sklar - git://gist.github.com/3249416.git
// Adafruit article: http://learn.adafruit.com/reading-a-analog-in-and-controlling-audio-volume-with-the-raspberry-pi
// Ported from python and modified by: Gilberto Garcia <ferraripr@gmail.com>; twitter: @ferraripr

namespace RaspberryPiDotNet
{
    /// <summary>
    /// Raspberry Pi using MCP3008 A/D Converters with SPI Serial Interface
    /// <seealso cref="http://ww1.microchip.com/downloads/en/DeviceDoc/21295d.pdf"/>
    /// </summary>
    public class MCP41010
    {
        private GPIO clockpin;
        private GPIO mosipin;
        private GPIO misopin;
        private GPIO cspin;

        /// <summary>
        /// Connect MCP3008 with clock, Serial Peripheral Interface(SPI) and channel
        /// </summary>
        /// <param name="SPICLK">Clock pin</param>
        /// <param name="SPIMOSI">SPI Master Output, Slave Input (MOSI)</param>
        /// <param name="SPIMISO">SPI Master Input, Slave Output (MISO)</param>
        /// <param name="SPICS">SPI Chip Select</param>
		public MCP41010(GPIO SPICLK, GPIO SPIMISO, GPIO SPIMOSI, GPIO SPICS)
        {
            clockpin = SPICLK;
            mosipin = SPIMOSI;
            cspin = SPICS;
        }

        /// <summary>
        /// Analog to digital conversion
        /// </summary>
		public void SetResistenza(int value)
        {
            cspin.Write(false);
			System.Threading.Thread.Sleep (1);

            clockpin.Write(true); // #start clock low
            cspin.Write(true); // #bring CS low

			int numBitSpediti = 0;
			Console.WriteLine ("inizio");
			System.Threading.Thread.Sleep (1);

			int commandout = 17;

			for (int i = 0; i < 8; i++)
			{
				if ((commandout & 0x80) == 128)
				{
					mosipin.Write(false);
				}
				else
				{
					mosipin.Write(true);
				}
				commandout <<= 1;
				clockpin.Write(false);
				System.Threading.Thread.Sleep (1);
				clockpin.Write(true);
				System.Threading.Thread.Sleep (1);
				numBitSpediti++;
			}

            commandout = value;

            for (int i = 0; i < 8; i++)
            {
                if ((commandout & 0x80) == 128)
                {
                    mosipin.Write(false);
                }
                else
                {
                    mosipin.Write(true);
                }
                commandout <<= 1;
                clockpin.Write(false);
				System.Threading.Thread.Sleep (1);
                clockpin.Write(true);
				System.Threading.Thread.Sleep (1);
				numBitSpediti++;
            }

            cspin.Write(false);
			System.Threading.Thread.Sleep (1);
			Console.WriteLine ("Trasmesso " + value);
        }

    }
}
