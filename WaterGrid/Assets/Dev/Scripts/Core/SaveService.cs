using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.Save
{
    public static class SaveService
    {
        private static string savePath;

        /// <summary>
        /// Save파일이 있는지 반환
        /// </summary>
        public static bool Exists
        {
            get
            {
                if (savePath == null)
                    return false;

                return File.Exists(savePath);
            }
        }

        /// <summary>
        /// 세이브 경로 설정 함수
        /// </summary>
        public static void SetSavePath(string path)
        {
            savePath = path;
        }

        /// <summary>
        /// 저장 함수
        /// </summary>
        public static void Save(string json)
        {
            File.WriteAllText(savePath, json);
        }

        /// <summary>
        /// 저장 함수
        /// </summary>
        public static void Save(string path, string json)
        {
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// 파일 불러오기 함수
        /// </summary>
        public static T Load<T>() where T : ILoadDatable
        {
            T data = default;
            data = JsonUtility.FromJson<T>(File.ReadAllText(savePath));

            return data;
        }

        /// <summary>
        /// 파일 불러오기 함수
        /// </summary>
        public static T Load<T>(string path) where T : ILoadDatable
        {
            T data = default;
            
            if(File.Exists(path) is false)
                return data;

            data = JsonUtility.FromJson<T>(File.ReadAllText(path));

            return data;
        }
    }
}