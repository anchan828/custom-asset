using System;
using UnityEngine;
using UnityEditor;

public class CustomAssetEditor : Editor
{
    AssetImporter assetImporter
    {
        get
        {
            var assetPath = AssetDatabase.GetAssetPath(target);
            return AssetImporter.GetAtPath(assetPath);
        }
    }

    /// <summary>
    /// 設定データの保存
    /// AssetImporter.userData に JSON 形式で保存する。
    /// </summary>
    /// <param name="settings">保存する設定データ</param>
    /// <returns></returns>
    public virtual bool SaveSettings(object settings)
    {
        try
        {
            assetImporter.userData = JsonUtility.ToJson(settings);
            AssetDatabase.ImportAsset(assetImporter.assetPath);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 設定データの読み込み
    /// </summary>
    /// <typeparam name="T">設定クラス</typeparam>
    /// <returns></returns>
    public virtual T LoadSettings<T>() where T : class, new()
    {
        T result;
        if (string.IsNullOrEmpty(assetImporter.userData))
            result = new T();
        else
            result = JsonUtility.FromJson<T>(assetImporter.userData);
        return result;
    }
}
