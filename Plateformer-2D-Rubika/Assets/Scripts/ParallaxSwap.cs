using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSwap : MonoBehaviour
{
    public enum Parallax { gardenParallax, catacombsParallax, termiteParallax };

    public Parallax parallax;

    [SerializeField] Transform garden, catacombs, termite;



    void Update()
    {
     
        
        if(parallax == Parallax.gardenParallax)
        {
            foreach(SpriteRenderer child in garden)
            {
                child.enabled = true;
            }
            foreach (SpriteRenderer child in catacombs)
            {
                child.enabled = false;
            }
            foreach (SpriteRenderer child in termite)
            {
                child.enabled = false;
            }
        }
        else if (parallax == Parallax.catacombsParallax)
        {
            foreach (SpriteRenderer child in garden)
            {
                child.enabled = false;
            }
            foreach (SpriteRenderer child in catacombs)
            {
                child.enabled = true;
            }
            foreach (SpriteRenderer child in termite)
            {
                child.enabled = false;
            }

        }
        else if(parallax == Parallax.termiteParallax)
        {
            foreach (SpriteRenderer child in garden)
            {
                child.enabled = false;
            }
            foreach (SpriteRenderer child in catacombs)
            {
                child.enabled = false;
            }
            foreach (SpriteRenderer child in termite)
            {
                child.enabled = true;
            }

        }


    }
}
