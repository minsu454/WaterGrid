using Common.Time;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.DotweenEx
{
    public sealed class DotweenEx
    {
        private float startValue;           //시작 값
        private float endValue;             //끝 값
        private float duration;             //지속 시간

        private bool loop = false;          //루프
        private LoopType type;              //루프 타입

        private event Action deleted;       //메모리 해제

        private event Action callback;      //콜백

        private float value;                //현재 값
        public float Value
        {
            get { return value; }
        }
        private float curTime;              //현재 시간

        public DotweenEx(float startValue, float duration, float endValue, Action deleted)
        {
            this.startValue = startValue;
            this.duration = duration;
            this.endValue = endValue;
            this.deleted = deleted;
        }
        
        /// <summary>
        /// 업데이트 함수
        /// </summary>
        public void OnUpdate()
        {
            curTime += TimeType.InGame.Get() * UnityEngine.Time.deltaTime;
            float time = Mathf.Clamp01(curTime / duration);

            value = Mathf.Lerp(startValue, endValue, time);

            if (time == 1f)
            {
                OnCompleted();
            }
        }

        /// <summary>
        /// 루프 설정 함수
        /// </summary>
        public DotweenEx SetLoop(LoopType loopType = LoopType.None)
        {
            loop = true;
            type = loopType;
            return this;
        }

        /// <summary>
        /// 콜백 함수
        /// </summary>
        public DotweenEx OnCompleted(Action callback)
        {
            this.callback = callback;
            return this;
        }

        /// <summary>
        /// 콜백 실행 함수
        /// </summary>
        private void OnCompleted()
        {
            callback?.Invoke();
            if (loop)
            {
                switch(type)
                {
                    case LoopType.None:
                        value = startValue;
                        break;
                    case LoopType.Yoyo:
                        endValue = startValue;
                        startValue = value;
                        break;
                }

                curTime = 0f;
            }
            else
            {
                deleted.Invoke();
            }
        }
    }
}
