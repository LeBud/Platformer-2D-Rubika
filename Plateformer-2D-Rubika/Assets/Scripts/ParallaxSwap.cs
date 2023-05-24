using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSwap : MonoBehaviour
{
    public enum Parallax { gardenParallax, catacombsParallax, termiteParallax };

    public Parallax parallax;

    [SerializeField] Transform garden, catacombs, termite;

    private void Start()
    {
        ParallaxSwapFonction();
    }

    public void ParallaxSwapFonction()
    {
        if (parallax == Parallax.gardenParallax)
        {
            foreach (Transform child in garden)
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
            }
            foreach (Transform child in catacombs)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (Transform child in termite)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if (parallax == Parallax.catacombsParallax)
        {
            foreach (Transform child in garden)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (Transform child in catacombs)
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
            }
            foreach (Transform child in termite)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }

        }
        else if (parallax == Parallax.termiteParallax)
        {
            foreach (Transform child in garden)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (Transform child in catacombs)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (Transform child in termite)
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
            }

        }

    }

}
