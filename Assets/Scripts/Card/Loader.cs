using System.Threading.Tasks;
using UnityEngine;

public abstract class Loader : MonoBehaviour
{
    public abstract Task<Texture2D>  LoadTexture(int sizeX, int sizeY, string url = "https://picsum.photos/");

}
