//#if UNITY_EDITOR
//using UnityEngine;
//using UnityEditor;
//using System.Diagnostics;

//[InitializeOnLoad]
//public static class GameObjectCreationLogger
//{
//    static GameObjectCreationLogger()
//    {
//        // Подписываемся на событие изменения иерархии
//        EditorApplication.hierarchyChanged += () =>
//        {
//            // Находим все объекты в сцене
//            foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
//            {
//                // Только объекты, реально в сцене
//                if (!obj.scene.IsValid()) continue;

//                // Флаг для первого обнаружения объекта
//                if (!obj.TryGetComponent<ObjectTag>(out _))
//                {
//                    obj.gameObject.AddComponent<ObjectTag>(); // "Помечаем", чтобы не логать снова

//                    // Выводим имя объекта + стек вызова
//                    StackTrace stack = new StackTrace(true);
//                    UnityEngine.Debug.Log($"Новый объект: {obj.name}\n{stack}", obj);
//                }
//            }
//        };
//    }

//    // Вспомогательный компонент для пометки объектов
//    private class ObjectTag : MonoBehaviour { }
//}
//#endif