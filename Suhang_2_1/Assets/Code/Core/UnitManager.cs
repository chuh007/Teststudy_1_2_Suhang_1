using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Code.Units.Unit;

namespace Code.Core
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private List<Unit> selectedUnits;

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
            if (isMouseDown)
            {
                Vector3 worldPosition = playerInput.GetWorldPosition(out RaycastHit hitInfo);
                // 마우스 월드좌표 가져옴
                if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Unit unit))
                {
                    // 충돌한게 있고, 충돌한 것에  Unit이라는 스크립트가 붙어있었으면
                    SelectUnit(unit);                       // 해당 유닛을 선택한다.
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
        }

        private void SelectUnit(Unit targetUnit)
        {
            // 다른 유닛을 선택하면 기존 유닛들의 선택을 전부 제거 해주고
            foreach (Unit u in selectedUnits)
            {
                u.SetSelected(false);
            }
            // selectedUnits.ForEach(unit => unit.SetSelected(false));
            // 선택 유닛을 비워준다.
            selectedUnits.Clear();
            
            // 현제 선택한 유닛을 넣어주고, 선택을 활성화해준다.
            selectedUnits.Add(targetUnit);
            targetUnit.SetSelected(true);
        }
    }
}