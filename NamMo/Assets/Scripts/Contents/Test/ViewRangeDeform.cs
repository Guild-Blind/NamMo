using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ViewRangeDeform : MonoBehaviour
{
    [SerializeField] private float _deformSpeed;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    Vector3[] _originVertices;
    Vector3[] _vertices;
    bool[] _deformCheck;
    HashSet<Collider2D> _colliders = new HashSet<Collider2D>();

    float[] _dirX = new float[12] { 0, 0.5f, 0.86f, 1, 0.86f, 0.5f, 0, -0.5f, -0.86f, -1, -0.86f, -0.5f };
    float[] _dirY = new float[12] { 1, 0.86f, 0.5f, 0, -0.5f, -0.86f, -1, -0.86f, -0.5f, 0, 0.5f, 0.86f };
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;

        GetVertices();
    }
    private void Update()
    {
        DeformVertices();
        UpdateVertices();
    }
    private void GetVertices()
    {
        _originVertices = new Vector3[_mesh.vertices.Length];
        _vertices = new Vector3[_mesh.vertices.Length];
        _deformCheck = new bool[_mesh.vertices.Length];
        for (int i = 0; i < _mesh.vertices.Length; i++)
        {
            _originVertices[i] = _mesh.vertices[i];
            //_originVertices[i].z = 0;
            _vertices[i] = _mesh.vertices[i];
            //_vertices[i].z = 0;
        }
    }
    private void DeformVertices()
    {
        for (int i = 0; i < _vertices.Length; i++)
        {
            _deformCheck[i] = false;
        }
        for (int i = 0; i < 12; i++) // 12���� ray Ž��
        {
            RaycastHit2D hit;
            float rayDist = 5f;
            hit = Physics2D.Raycast(transform.position, new Vector2(_dirX[i], _dirY[i]), rayDist, LayerMask.GetMask("Ground")); // Ground Layer�� Ž��
            if (hit)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);

                // local�� ������ world�������� �ٲٰ� Ray�� Ž���� ������Ʈ ���� ���� ����� ��ǥ�� ����
                Vector2 closestPos = hit.collider.ClosestPoint(transform.TransformPoint(Vector2.zero));
                // Ž���� ���� ����� ��ǥ�� local�� ��ȯ
                Vector2 localClosestPos = transform.InverseTransformPoint(closestPos);
                // �浹 ������ local ��ǥ��� ��ȯ
                Vector2 localHitPoint = transform.InverseTransformPoint(hit.point);
                // ���� �� ��ǥ�� ���� ������ ���͸� ���Ѵ�
                Vector2 localHitObjectVector = localClosestPos - localHitPoint;
                if (localHitObjectVector.magnitude > 0.03f) localHitObjectVector = localHitObjectVector.normalized;
                // ������ ���� ���� ���̸� ���Ѵ�
                float projection = Vector2.Dot(-localHitPoint, localHitObjectVector);
                // �ִ� ���͸� ���Ѵ�. (�������� ������ �������� ����)
                Vector2 projectedPoint = localHitPoint + localHitObjectVector * projection;
                for (int j = 0; j < _vertices.Length; j++)
                {
                    // �浹 ���������� ���Ϳ� ��� ����������� ���͸� �˻�
                    float value = Vector2.Angle(new Vector2(localHitPoint.x, localHitPoint.y), new Vector2(_originVertices[j].x, _originVertices[j].y));
                    float maxAngle = 15f;
                    // ������ 15�� �̻��̸� ��ŵ, �ٸ� ������ �ñ��.
                    if (value > maxAngle) continue;

                    // �ִ� ���Ϳ� ���������� ������ ���̰��� ���Ѵ�.
                    float angle = Vector2.Angle(projectedPoint, new Vector2(_originVertices[j].x, _originVertices[j].y));

                    // �ﰢ�Լ� �����ϸ� �̷� ���� ���� �� ����
                    float vertexLen = (projectedPoint.magnitude) / Mathf.Cos(angle * Mathf.Deg2Rad);
                    vertexLen /= rayDist;
                    vertexLen = Mathf.Clamp(vertexLen, 0, 1);
                    _vertices[j] = Vector3.Slerp(_vertices[j], _originVertices[j] * vertexLen, _deformSpeed);
                    _deformCheck[j] = true;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + new Vector3(_dirX[i], _dirY[i]) * 5f, Color.red);
            }
        }
        for (int i = 0; i < _vertices.Length; i++)
        {
            if (_deformCheck[i] == false)
            {
                _vertices[i] = Vector3.Slerp(_vertices[i], _originVertices[i], _deformSpeed);
            }
        }
    }
    private void UpdateVertices()
    {
        _mesh.vertices = _vertices;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer != LayerMask.GetMask("Ground")) return;
        SpriteRenderer spr = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spr == null) return;
        _colliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer != LayerMask.GetMask("Ground")) return;
        SpriteRenderer spr = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spr == null) return;
        _colliders.Remove(collision);
    }
}
