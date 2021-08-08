using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetLoader : Loader
{
    public override async Task<Texture2D> LoadTexture(int sizeX, int sizeY, string url = "https://picsum.photos/")
    {
        Texture2D texture = new Texture2D(sizeX,sizeY);
        texture = await GetRemoteTexture(url + sizeX + "/" + sizeY);
        return texture;
    }

    public static async Task<Texture2D> GetRemoteTexture ( string url )
    {
        using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while( asyncOp.isDone==false )
                await Task.Delay( 1000/30 );//30 hertz
        
            // read results:
            if( www.isNetworkError || www.isHttpError )
                // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                // log error:
#if DEBUG
                Debug.Log( $"{www.error}, URL:{www.url}" );
#endif
            
                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }

}
