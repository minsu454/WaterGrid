using Common.Time;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Common.DotweenEx
{
    public sealed class DotweenEx
    {
        private float startValue;           //시작 값
        private float endValue;             //끝 값
        private float duration;             //지속 시간
        private TimeType timeType;          //시간 흐름 타입

        private bool loop = false;          //루프
        private LoopType loopType;          //루프 타입

        private event Action deleted;       //메모리 해제

        private event Action callback;      //콜백

        private float value;                //현재 값
        public float Value
        {
            get { return value; }
        }
        private float curTime;              //현재 시간
        private float basicTime = 1;        //기본 시간
            
        public DotweenEx(float startValue, float duration, float endValue, TimeType timeType, Action deleted)
        {
            this.startValue = startValue;
            this.duration = duration;
            this.endValue = endValue;
            this.deleted = deleted;
            this.timeType = timeType;
        }
        
        /// <summary>
        /// 업데이트 함수
        /// </summary>
        public void OnUpdate()
        {
            curTime += basicTime * timeType.Get() * UnityEngine.Time.deltaTime;
            float time = Mathf.Clamp01(curTime / duration);

            value = Mathf.Lerp(startValue, endValue, time);

            if (time == 1f)
            {
                OnCompleted();
            }
        }

        public void Reset()
        {
            value = startValue;
            curTime = 0f;
            basicTime = 1;
        }

        public void ReStart()
        {
            Reset();
        }

        public void Stop()
        {
            basicTime = 0;
        }

        /// <summary>
        /// 루프 설정 함수
        /// </summary>
        public DotweenEx SetLoop(LoopType loopType = LoopType.None)
        {
            loop = true;
            this.loopType = loopType;
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
                switch(loopType)
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
                SafeDeleteAsync().Forget();
            }
        }

        private async UniTaskVoid SafeDeleteAsync()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            deleted?.Invoke();
        }
    }
}
