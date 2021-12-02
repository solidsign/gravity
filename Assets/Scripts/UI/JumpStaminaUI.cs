using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class JumpStaminaUI : MonoBehaviour
    {
        [SerializeField] private Image jumpBlobPrefab;
        [SerializeField] private Vector2 nextBlobOffset;
        [Range(0f, 1f)] [SerializeField] private float showAlpha; 
        [Range(0f, 1f)] [SerializeField] private float hideAlpha; 
        private List<Image> _images;
        private Vector2 _offset;
        public void Init(int jumpsAmount)
        {
            _images = new List<Image>(jumpsAmount);
            _offset = Vector2.zero;
            for (int i = 0; i < jumpsAmount; i++)
            {
                var im = Instantiate(jumpBlobPrefab, transform);
                im.rectTransform.position += (Vector3)_offset;
                _images.Add(im);
                _offset += nextBlobOffset;
            }
        }

        public void AddJumps(int jumpsAmount)
        {
            for (int i = 0; i < jumpsAmount; i++)
            {
                var im = Instantiate(jumpBlobPrefab, transform);
                im.rectTransform.position += (Vector3)_offset;
                _images.Add(im);
                _offset += nextBlobOffset;
            }
        }

        public void DeleteJumps(int jumpsAmount)
        {
            if (jumpsAmount > _images.Count) jumpsAmount = _images.Count;
            var newSize = _images.Count - jumpsAmount;
            foreach (var image in _images)
            {
                image.DOKill();
                Destroy(image.gameObject);
            }
            Init(newSize);
        }

        public void SetStamina(float stamina)
        {
            foreach (var image in _images)
            {
                if (stamina >= 1f)
                {
                    image.fillAmount = 1f;
                    stamina -= 1f;
                }
                else if(stamina > 0f)
                {
                    image.fillAmount = stamina;
                    stamina = 0f;
                }
                else
                {
                    image.fillAmount = 0f;
                }
            }
        }

        public void Show(float time)
        {
            foreach (var image in _images)
            {
                image.DOKill();
                image.DOFade(showAlpha, time).SetAutoKill();
            }
        }

        public void Hide(float time)
        {
            foreach (var image in _images)
            {
                image.DOKill();
                image.DOFade(hideAlpha, time).SetAutoKill();
            }
        }
    }
}