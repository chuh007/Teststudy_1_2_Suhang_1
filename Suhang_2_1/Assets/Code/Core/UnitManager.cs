using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEngine;
using Unit = Code.Units.Unit;

namespace Code.Core
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private List<Unit> selectedUnits;

        private Vector3 _dragStartPosition;
        private Vector3 _dragEndPosition;
        
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
                _dragStartPosition = worldPosition;
                // 마우스 월드좌표 가져옴
                if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Unit unit))
                {
                    // 충돌한게 있고, 충돌한 것에  Unit이라는 스크립트가 붙어있었으면
                    SelectUnit(unit);                       // 해당 유닛을 선택한다.
                }
                else if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    AttackCommendUnit(hitInfo.transform);
                }
                else if (selectedUnits.Count > 0)
                {
                    // 충돌한게 없는데, 내가 선택하고 있던 유닛이 있다면 해당 유닛에게 명령을 내린다.
                    //  여기는 차후에 공격명력과 구분해야 함.(위쪽에 else if)
                    foreach (Unit u in selectedUnits)
                    {
                        u.MoveToPosition(worldPosition);
                    }
                    // selectedUnits.ForEach(u => u.MoveToPosition(worldPosition));
                }
            }
            else
            {
                _dragEndPosition = worldPosition;
                Debug.Log(_dragStartPosition);
                Debug.Log(_dragEndPosition);

                Vector3 center = (_dragStartPosition + _dragEndPosition) / 2f;
                Vector3 halfExtents = new Vector3(
                    Mathf.Abs(_dragEndPosition.x - _dragStartPosition.x) / 2f,
                    10f,
                    Mathf.Abs(_dragEndPosition.z - _dragStartPosition.z) / 2f
                );

                Collider[] colliders = Physics.OverlapBox(center, halfExtents);
                List<Unit> units = new List<Unit>();

                foreach (var item in colliders)
                {
                    Debug.Log(item.name);
                    if (item.TryGetComponent(out Unit unit))
                    {
                        units.Add(unit);
                    }
                }

                SelectUnit(units);
            }
        }
        private void OnDrawGizmos()
        {
            if (_dragStartPosition != Vector3.zero && _dragEndPosition != Vector3.zero)
            {
                Vector3 center = (_dragStartPosition + _dragEndPosition) / 2f;
                Vector3 halfExtents = new Vector3(
                    Mathf.Abs(_dragEndPosition.x - _dragStartPosition.x) / 2f,
                    1f,
                    Mathf.Abs(_dragEndPosition.z - _dragStartPosition.z) / 2f
                );

                Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // 반투명 초록색
                Gizmos.DrawCube(center, halfExtents * 2f); // 주의: DrawCube는 전체 크기를 요구
            }
        }
        private void SelectUnit(Unit targetUnit)
        {
            foreach (Unit u in selectedUnits)
            {
                u.SetSelected(false);
            }
            selectedUnits.Clear();
            selectedUnits.Add(targetUnit);
            targetUnit.SetSelected(true);
        }

        private void SelectUnit(List<Unit> targetUnits)
        {
            foreach (Unit u in selectedUnits)
            {
                u.SetSelected(false);
            }
            selectedUnits.Clear();
            foreach (var unit in targetUnits)
            {
                selectedUnits.Add(unit);
                unit.SetSelected(true);
            }
        }

        private void AttackCommendUnit(Transform enemy)
        {
            foreach (Unit unit in selectedUnits)
            {
                unit.Attack(enemy);
            }
        }
    }
}