using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Unit = Code.Units.Unit;

namespace Code.Core
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private List<Unit> selectsUnits;
        [SerializeField] private float dragThreshold = 1f;
        
        private Vector3 _dragWorldStartPos;
        private Vector3 _dragWorldEndPos;
        
        private void Awake()
        {
            playerInput.OnMouseStatusChange += HandleMouseStatusChange;
        }


        private void OnDestroy()
        {
            playerInput.OnMouseStatusChange -= HandleMouseStatusChange;
        }

        private void HandleMouseStatusChange(bool isMouseDown)
        {
            Vector3 worldPosition = playerInput.GetWorldPosition(out RaycastHit hitInfo);
            if (isMouseDown)
            {
                _dragWorldStartPos = worldPosition;
                // 마우스 월드좌표 가져옴
            }
            else
            {
                _dragWorldEndPos = worldPosition;
                if (Vector3.Distance(_dragWorldStartPos, _dragWorldEndPos) < dragThreshold)
                    MouseClick(hitInfo, worldPosition);
                else
                    MouseDrag();
            }
        }

        private void MouseClick(RaycastHit hitInfo, Vector3 worldPosition)
        {
            if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Unit unit))
            {
                // 충돌한게 있고, 충돌한 것에  Unit이라는 스크립트가 붙어있었으면
                SelectUnit(unit);                       // 해당 유닛을 선택한다.
            }
            else if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                AttackCommendUnit(hitInfo.transform);
            }
            else if (selectsUnits.Count > 0)
            {
                foreach (Unit u in selectsUnits)
                {
                    u.MoveToPosition(worldPosition);
                }
            }
        }
        
        private void MouseDrag()
        {
            List<Unit> units = new List<Unit>();

            // 드래그 박스 계산 (화면 좌표 기준)
            Vector2 screenStart = Camera.main.WorldToScreenPoint(_dragWorldStartPos);
            Vector2 screenEnd = Camera.main.WorldToScreenPoint(_dragWorldEndPos);

            Rect dragRect = new Rect(
                Mathf.Min(screenStart.x, screenEnd.x),
                Mathf.Min(screenStart.y, screenEnd.y),
                Mathf.Abs(screenStart.x - screenEnd.x),
                Mathf.Abs(screenStart.y - screenEnd.y)
            );

            Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
            foreach (Unit unit in allUnits)
            {
                Vector2 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
                if (dragRect.Contains(unitScreenPos, true))
                {
                    units.Add(unit);
                }
            }

            SelectUnit(units);
        }

        private void SelectUnit(Unit targetUnit)
        {
            foreach (Unit u in selectsUnits)
            {
                u.SetSelected(false);
            }
            selectsUnits.Clear();
            selectsUnits.Add(targetUnit);
            targetUnit.SetSelected(true);
        }

        private void SelectUnit(List<Unit> targetUnits)
        {
            foreach (Unit u in selectsUnits)
            {
                u.SetSelected(false);
            }
            selectsUnits.Clear();
            foreach (var unit in targetUnits)
            {
                selectsUnits.Add(unit);
                unit.SetSelected(true);
            }
        }

        private void AttackCommendUnit(Transform enemy)
        {
            foreach (Unit unit in selectsUnits)
            {
                unit.Attack(enemy);
            }
        }
    }
}