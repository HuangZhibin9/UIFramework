using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// UI面板集合，记录当前场景中的所有面板
        /// </summary>
        public static Dictionary<Type, UIPanelBase> PanelDic = null;

        /// <summary>
        /// UI画布
        /// </summary>
        public static Canvas Canvas { get; private set; } = null;

        /// <summary>
        /// UI事件系统
        /// </summary>
        public static EventSystem EventSystem { get; private set; } = null;

        /// <summary>
        /// UIManager单例，全局唯一
        /// </summary>
        public static UIManager Instance { get; private set; } = null;

        /// <summary>
        /// UI正交相机
        /// </summary>
        public static Camera Camera { get; private set; } = null;

        /// <summary>
        /// UIRoot层级合集
        /// </summary>
        private static Dictionary<UIPanelLayer, RectTransform> layers = null;

        /// <summary>
        /// 获取指定的UI层级
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static RectTransform GetLayer(UIPanelLayer layer)
        {
            return layers[layer];
        }

        /// <summary>
        /// 初始化UI框架
        /// </summary>
        public static void Init()
        {
            PanelDic = new Dictionary<Type, UIPanelBase>();

            // 创建UIRoot
            var obj = Resources.Load("Prefabs/UIRoot/UIRoot");
            Instance = GameObject.Instantiate(obj).AddComponent<UIManager>();
            Instance.name = nameof(UIManager);
            DontDestroyOnLoad(Instance);

            Canvas = Instance.GetComponentInChildren<Canvas>();
            EventSystem = Instance.GetComponentInChildren<EventSystem>();
            Camera = Instance.GetComponentInChildren<Camera>();

            //获取所有层级
            layers = new Dictionary<UIPanelLayer, RectTransform>();
            foreach (UIPanelLayer layer in Enum.GetValues(typeof(UIPanelLayer)))
            {
                layers.Add(layer, Canvas.transform.Find(layer.ToString()) as RectTransform);
            }

            Debug.Log("Init success");
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="T">准备打开的面板类型</typeparam>
        /// <param name="data">需要的数据</param>
        public static void Open<T>(object data = null) where T : UIPanelBase
        {
            //打开面板
            void openPanel(UIPanelBase panel)
            {
                //将面板设置为当前层级的最后位，以优先显示该面板
                panel.transform.SetAsLastSibling();

                panel.SetData(data);
                panel.OnUIEnable();
            }

            //克隆面板
            UIPanelBase clonePanel()
            {
                //查找面板路径
                var panelRootPath = "Prefabs/UI";
                var panelSubPath = $"{typeof(T).Name}/{typeof(T).Name}";
                var panelPath = $"{panelRootPath}/{panelSubPath}";

                //加载面板
                var panelPrefab = Resources.Load(panelPath);
                if (panelPrefab == null)
                {
                    Debug.Log($"该路径下:{panelPath}未找到此面板:{typeof(T).Name}");
                }

                //查找面板所在层级
                RectTransform layer = UIManager.GetLayer(UIPanelLayer.Normal);

                var attributes = typeof(T).GetCustomAttributes(typeof(UILayerAttribute), true);
                if (attributes.Length > 0)
                {
                    var layerAttribute = attributes[0] as UILayerAttribute;
                    layer = UIManager.GetLayer(layerAttribute.layer);
                }

                //克隆面板
                var newPanel = Instantiate(panelPrefab, layer).AddComponent<T>();
                newPanel.gameObject.name = typeof(T).Name;
                return newPanel;
            }

            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                //如果面板集合中已经存在该面板，则直接显示
                openPanel(panel);
            }
            else
            {
                //克隆面板
                panel = clonePanel();

                //将面板加入字典
                PanelDic.Add(typeof(T), panel);
                panel.OnUIAwake();
                //延迟一帧之后执行 OnUIStart()
                Instance.StartCoroutine(Invoke(panel.OnUIStart));

                //显示面板
                openPanel(panel);
            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <typeparam name="T">需要关闭的面板类型</typeparam>
        public static void Close<T>() where T : UIPanelBase
        {
            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                UIManager.Close(panel);
            }
            else
            {
                Debug.LogError($"未找到此面板:{typeof(T).Name}");
            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="panel">需要关闭的面板实例</param>
        public static void Close(UIPanelBase panel)
        {
            panel?.OnUIDisable();
        }

        /// <summary>
        /// 关闭所有面板
        /// </summary>
        public static void CloseAll()
        {
            foreach (var panel in PanelDic.Values)
            {
                UIManager.Close(panel);
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <typeparam name="T">需要销毁的面板类型</typeparam>
        public static void Destory<T>() where T : UIPanelBase
        {
            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                UIManager.Destory(panel);
            }
            else
            {
                Debug.LogError($"未找到此面板:{typeof(T).Name}");
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="panel">需要销毁的面板实例</param>
        public static void Destory(UIPanelBase panel)
        {
            panel?.OnUIDisable();
            panel?.OnUIDestory();

            //移除字典中的数据
            if (PanelDic.ContainsKey(panel.GetType()))
            {
                PanelDic.Remove(panel.GetType());
            }
        }

        /// <summary>
        /// 销毁所有面板
        /// </summary>
        public static void DestoryAll()
        {
            List<UIPanelBase> panels = new List<UIPanelBase>(PanelDic.Values);
            foreach (var panel in panels)
            {
                UIManager.Destory(panel);
            }
            PanelDic.Clear();
            panels.Clear();
        }

        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T">想要获取的面板类型</typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : UIPanelBase
        {
            if (PanelDic.TryGetValue(typeof(T), out UIPanelBase panel))
            {
                return panel as T;
            }
            else
            {
                Debug.LogError($"未找到此面板:{typeof(T).Name}");
            }
            return default(T);
        }

        /// <summary>
        /// 延迟一帧执行
        /// </summary>
        /// <param name="callback">需要延迟一帧执行的回调</param>
        /// <returns></returns>
        private static IEnumerator Invoke(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
    }
}