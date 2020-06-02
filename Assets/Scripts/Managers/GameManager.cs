using System;
using System.Collections;
using System.Collections.Generic;
using HexagonAli.Data;
using HexagonAli.Helpers;
using HexagonAli.View;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class GameManager : LocalSingleton<GameManager>
    {
        public event Action OnHexagonExplode;
        public event Action OnMoveChanged;
        public event Action OnThousandScoreReached;

        private HexagonGroup _selectedGroup;

        private int _score;
        private int _moveCount;
        private int _nextBombScore;
        private bool _gameOver;
        private bool _restarting;
        private bool _rotating;
        private bool _anyGroupSelected;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            ColorReferencer.Instance.Init();
            CameraManager.Instance.Init();
            InputManager.Instance.OnTap += OnTap;
            InputManager.Instance.OnSwipe += OnSwipe;
            HexagonManager.Instance.Initialize(GameConfiguration.Instance.columnCount, GameConfiguration.Instance.rowCount);
            if(ColorReferencer.Instance.GetColorCount() <= 2)
            {
                Debug.LogWarning("You need to put 3 colors at least in Game Configuration.");
            }
            else
            {
                HexagonManager.Instance.GenerateHexagons();
                HexagonGroupManager.Instance.CalculateGroups(GameConfiguration.Instance.columnCount, GameConfiguration.Instance.rowCount);
            }
            UIManager.Instance.UpdateTexts();
            _nextBombScore = GameConfiguration.Instance.bombScore;
            yield return FaderManager.Instance.OpenTheater();
        }

        public void OnHexagonExploded()
        {
            _score += 5;
            if (_score >= _nextBombScore)
            {
                _nextBombScore += GameConfiguration.Instance.bombScore ;
                HexagonManager.Instance.SetNextBombFlag(true);
                OnThousandScoreReached?.Invoke();
            }
            OnHexagonExplode?.Invoke();
        }

        public void LoseGame(GameOverReason gameOverReason )
        {
            if(_gameOver)
            {
                return;
            }
            _gameOver = true;
            if(gameOverReason == GameOverReason.NoPossibleMove)
            {
                UIManager.Instance.SetGameOverReason("No Possible Move");
            }
            else if (gameOverReason == GameOverReason.BombExploded)
            {
                UIManager.Instance.SetGameOverReason("Bomb Exploded");
            }
            StartCoroutine(LoseGameRoutine());
        
        }

        private void IncreaseMoveCount()
        {
            _moveCount++;
            OnMoveChanged?.Invoke();
        }

        private void OnTap(Vector3 pos)
        {
            if(_gameOver || _restarting || _rotating)
            {
                return;
            }
            HexagonGroup group = HexagonGroupManager.Instance.NearestGroupTo(pos + (Vector3)HexagonManager.Instance.CenterOffset());
            float distance = Vector3.Distance(HexagonManager.Instance.PixelToWorld(group.GetCenter()), pos);
            //checking if tapping other than grid.
            if(distance < 1f)
            {
                _selectedGroup = HexagonGroupManager.Instance.NearestGroupTo(pos + (Vector3)HexagonManager.Instance.CenterOffset());
                HexagonGroupRotator.Instance.SetHexagonGroup(_selectedGroup);
                _anyGroupSelected = true;
            }
        
        }

        private void OnSwipe(Vector3 fingerPos, Vector2 swipeDelta)
        {
            if (_gameOver || _restarting || _rotating || !_anyGroupSelected)
            {
                return;
            }
            Vector3 groupPos = HexagonManager.Instance.PixelToWorld(_selectedGroup.GetCenter());
            
            //checking if finger position is above than selected group
            if (fingerPos.y > groupPos.y)
            {
                //checking if swipe left
                if(swipeDelta.x < 0)
                {
                    StartCoroutine(RotateGroup(false, 0.2f));
                }
                else
                {
                    StartCoroutine(RotateGroup(true, 0.2f));
                }
            }
            else
            {
                if (swipeDelta.x < 0)
                {
                    StartCoroutine(RotateGroup(true, 0.2f));
                }
                else
                {
                    StartCoroutine(RotateGroup(false, 0.2f));
                }
            }
            //Debug.Log("Finger Pos : " + fingerPos);
            //Debug.Log("Group Pos : " + HexagonManager.Instance.PixelToWorld(_selectedGroup.GetCenter()));
            //Debug.Log("Swipe Delta : " + swipeDelta);
        }

        public void TryAgain()
        {
            StartCoroutine(RestartGame());
        }

        IEnumerator LoseGameRoutine()
        {
            yield return new WaitForSeconds(1f);
            UIManager.Instance.ShowGameOverPanel();
        }

        IEnumerator RestartGame()
        {
            _restarting = true;
            yield return FaderManager.Instance.CloseTheater();
            HexagonManager.Instance.KillAllHexagons();
            HexagonManager.Instance.GenerateHexagons();
            HexagonManager.Instance.ClearBombHexagonList();
            UIManager.Instance.HideGameOverPanel();
            yield return FaderManager.Instance.OpenTheater();
            _gameOver = false;
            _score = 0;
            _moveCount = 0;
            _nextBombScore = GameConfiguration.Instance.bombScore;
            UIManager.Instance.UpdateTexts();
            _restarting = false;
        }
    
        IEnumerator RotateGroup(bool clockwise, float duration)
        {
            _rotating = true;
            for (int i = 0; i < 3; i++)
            {
                Hexagon a = HexagonManager.Instance.GetHexagon(_selectedGroup.A);
                Hexagon b = HexagonManager.Instance.GetHexagon(_selectedGroup.B);
                Hexagon c = HexagonManager.Instance.GetHexagon(_selectedGroup.C);
                if (clockwise)
                {
                    //swapping hexagons in group
                    HexagonManager.Instance.SetHexagon(_selectedGroup.B, a);
                    HexagonManager.Instance.SetHexagon(_selectedGroup.C, b);
                    HexagonManager.Instance.SetHexagon(_selectedGroup.A, c);
                }
                else
                {
                    //swapping hexagons in group
                    HexagonManager.Instance.SetHexagon(_selectedGroup.C, a);
                    HexagonManager.Instance.SetHexagon(_selectedGroup.B, c);
                    HexagonManager.Instance.SetHexagon(_selectedGroup.A, b);
                }
            

                HexagonGroupRotator.Instance.ParentSelectedHexagons();
                yield return HexagonGroupRotator.Instance.Rotate(clockwise, 120f, duration);
                HexagonGroupRotator.Instance.UnparentSelectedHexagons();
                yield return new WaitForSeconds(0.1f);
                bool anyMatch = false;
                while(HexagonGroupManager.Instance.CheckForMatchedGroups())
                {
                    HexagonGroupManager.Instance.ExplodeMatchedGroups();
                    HexagonGroupRotator.Instance.Reset();
                    HexagonManager.Instance.ShiftColumns();
                    yield return new WaitForSeconds(0.6f);
                    anyMatch = true;
                }
                if(anyMatch)
                {
                    HexagonManager.Instance.CountdownBombHexagons();
                    IncreaseMoveCount();
                    break;
                }
            }
            if(!AreThereAnyPossibleMoves())
            {
                LoseGame(GameOverReason.NoPossibleMove);
            }
            HexagonGroupRotator.Instance.SetHexagonGroup(_selectedGroup);
            _rotating = false;

        }

        public int GetScore()
        {
            return _score;
        }

        public int GetMoveCount()
        {
            return _moveCount;
        }
        
        bool AreThereAnyPossibleMoves()
        {
            //Getting groups that has got 2 same colors.
            List<PairedGroup> pairedGroups = HexagonGroupManager.Instance.GetAllPairedGroups();
            for (int i = 0; i < pairedGroups.Count; i++)
            {
                //Getting neighbor groups that has got only 1 common hexagon.
                List<HexagonGroup> singleCommonGroups = HexagonGroupManager.Instance.GetNeighbours(pairedGroups[i], 1);
                for (int j = 0; j < singleCommonGroups.Count; j++)
                {
                    //If 2 groups shares only 1 hexagon, singleCommonGroups[i] should have also 1 same color.
                    if (HexagonGroupManager.Instance.ColorCountOfGroup(singleCommonGroups[j], pairedGroups[i].ColorIndex) >= 1)
                    {
                        return true;
                    }
                }

                //Getting neighbor groups that has got 2 common hexagons.
                List<HexagonGroup> doubleCommonGroups = HexagonGroupManager.Instance.GetNeighbours(pairedGroups[i], 2);
                for (int j = 0; j < doubleCommonGroups.Count; j++)
                {
                    //If 2 groups shares 2 hexagons, doubleCommonGroups[i] should have 2 same colors at least because one of the hexagons is same.
                    if (HexagonGroupManager.Instance.ColorCountOfGroup(doubleCommonGroups[j], pairedGroups[i].ColorIndex) >= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }




    }
}
