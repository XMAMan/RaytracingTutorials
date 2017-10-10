using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytracingTutorials
{
    class CornellBoxSceneData
    {
        public Triangle[] Triangles = new Triangle[]
        {
            new Triangle(new Vector(0.343f,0.545f,0.332f), new Vector(0.213f,0.545f,0.227f), new Vector(0.343f,0.545f,0.227f), new Vector(1,1,1)), //Lampe
            new Triangle(new Vector(0.213f,0.545f,0.227f), new Vector(0.343f,0.545f,0.332f), new Vector(0.213f,0.545f,0.332f), new Vector(1,1,1)),

            new Triangle(new Vector(0.556f,0.000f,0.000f), new Vector(0.006f,0.000f,0.559f), new Vector(0.556f,0.000f,0.559f), new Vector(0.7f,0.7f,0.7f)), //Fußboden
            new Triangle(new Vector(0.006f,0.000f,0.559f), new Vector(0.556f,0.000f,0.000f), new Vector(0.003f,0.000f,0.000f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.556f,0.000f,0.559f), new Vector(0.000f,0.549f,0.559f), new Vector(0.556f,0.549f,0.559f), new Vector(0.7f,0.7f,0.7f)), //Rückwand
            new Triangle(new Vector(0.000f,0.549f,0.559f), new Vector(0.556f,0.000f,0.559f), new Vector(0.006f,0.000f,0.559f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.006f,0.000f,0.559f), new Vector(0.000f,0.549f,0.000f), new Vector(0.000f,0.549f,0.559f), new Vector(0.7f,0.2f,0.2f)), //Linke Wand
            new Triangle(new Vector(0.000f,0.549f,0.000f), new Vector(0.006f,0.000f,0.559f), new Vector(0.003f,0.000f,0.000f), new Vector(0.7f,0.2f,0.2f)),
                                                                                                                                                         
            new Triangle(new Vector(0.556f,0.000f,0.000f), new Vector(0.556f,0.549f,0.559f), new Vector(0.556f,0.549f,0.000f), new Vector(0.2f,0.7f,0.2f)), //Rechte Wand
            new Triangle(new Vector(0.556f,0.549f,0.559f), new Vector(0.556f,0.000f,0.000f), new Vector(0.556f,0.000f,0.559f), new Vector(0.2f,0.7f,0.2f)),
                                                                                                                                                         
            new Triangle(new Vector(0.556f,0.549f,0.559f), new Vector(0.000f,0.549f,0.000f), new Vector(0.556f,0.549f,0.000f), new Vector(0.7f,0.7f,0.7f)), // Decke
            new Triangle(new Vector(0.000f,0.549f,0.000f), new Vector(0.556f,0.549f,0.559f), new Vector(0.000f,0.549f,0.559f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                                                                                                                                                                                                                                                                                                                          
            new Triangle(new Vector(0.474f,0.165f,0.225f), new Vector(0.426f,0.165f,0.065f), new Vector(0.316f,0.165f,0.272f), new Vector(0.7f,0.7f,0.7f)),//Rechter Würfel Oben
            new Triangle(new Vector(0.266f,0.165f,0.114f), new Vector(0.316f,0.165f,0.272f), new Vector(0.426f,0.165f,0.065f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.316f,0.165f,0.272f), new Vector(0.266f,0.165f,0.114f), new Vector(0.266f,0.000f,0.114f), new Vector(0.7f,0.7f,0.7f)),//Rechter Würfel Links
            new Triangle(new Vector(0.316f,0.165f,0.272f), new Vector(0.266f,0.000f,0.114f), new Vector(0.316f,0.000f,0.272f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.316f,0.000f,0.272f), new Vector(0.316f,0.165f,0.272f), new Vector(0.474f,0.165f,0.225f), new Vector(0.7f,0.7f,0.7f)),//Rechter Würfel Hinten
            new Triangle(new Vector(0.474f,0.000f,0.225f), new Vector(0.316f,0.000f,0.272f), new Vector(0.474f,0.165f,0.225f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.426f,0.165f,0.065f), new Vector(0.474f,0.165f,0.225f), new Vector(0.474f,0.000f,0.225f), new Vector(0.7f,0.7f,0.7f)),//Rechter Würfel rechts
            new Triangle(new Vector(0.474f,0.000f,0.225f), new Vector(0.426f,0.000f,0.065f), new Vector(0.426f,0.165f,0.065f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.266f,0.165f,0.114f), new Vector(0.426f,0.165f,0.065f), new Vector(0.426f,0.000f,0.065f), new Vector(0.7f,0.7f,0.7f)),//Rechter Würfel vorne
            new Triangle(new Vector(0.426f,0.000f,0.065f), new Vector(0.266f,0.000f,0.114f), new Vector(0.266f,0.165f,0.114f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
			                                                                                                                                             
            new Triangle(new Vector(0.242f,0.330f,0.456f), new Vector(0.291f,0.330f,0.296f), new Vector(0.133f,0.330f,0.247f), new Vector(0.7f,0.7f,0.7f)),//Linker Würfel Oben
            new Triangle(new Vector(0.133f,0.330f,0.247f), new Vector(0.084f,0.330f,0.406f), new Vector(0.242f,0.330f,0.456f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.084f,0.330f,0.406f), new Vector(0.133f,0.330f,0.247f), new Vector(0.133f,0.000f,0.247f), new Vector(0.7f,0.7f,0.7f)),//Linker Würfel Links
            new Triangle(new Vector(0.133f,0.000f,0.247f), new Vector(0.084f,0.000f,0.406f), new Vector(0.084f,0.330f,0.406f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.242f,0.330f,0.456f), new Vector(0.084f,0.330f,0.406f), new Vector(0.084f,0.000f,0.406f), new Vector(0.7f,0.7f,0.7f)),//Linker Würfel Hinten
            new Triangle(new Vector(0.084f,0.000f,0.406f), new Vector(0.242f,0.000f,0.456f), new Vector(0.242f,0.330f,0.456f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.242f,0.000f,0.456f), new Vector(0.242f,0.330f,0.456f), new Vector(0.291f,0.330f,0.296f), new Vector(0.7f,0.7f,0.7f)),//Linker Würfel Rechts
            new Triangle(new Vector(0.291f,0.330f,0.296f), new Vector(0.291f,0.000f,0.296f), new Vector(0.242f,0.000f,0.456f), new Vector(0.7f,0.7f,0.7f)),
                                                                                                                                                         
            new Triangle(new Vector(0.133f,0.330f,0.247f), new Vector(0.291f,0.330f,0.296f), new Vector(0.291f,0.000f,0.296f), new Vector(0.7f,0.7f,0.7f)),//Linker Würfel vorne
            new Triangle(new Vector(0.291f,0.000f,0.296f), new Vector(0.133f,0.000f,0.247f), new Vector(0.133f,0.330f,0.247f), new Vector(0.7f,0.7f,0.7f)),
        };

        public LightSource LightSource;

        public Camera Camera;

        public CornellBoxSceneData(int screenWidth, int screenHeight)
        {
            this.Camera = new Camera(new Vector(0.278f, 0.275f, -0.789f), new Vector(0, 0, 1), new Vector(0, -1, 0), 38, screenWidth, screenHeight);
            this.LightSource = new LightSource(new Triangle[] { this.Triangles[0], this.Triangles[1] }, 1);
        }
    }
}
