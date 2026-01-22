using UnityEngine;

/// <summary>
/// 簡單墜落 - 純粹的物理運動，沒有複雜邏輯
/// </summary>
public class SimpleFall : MonoBehaviour
{
    [Header("基本設置")]
    public float startHeight = 10f;
    public float gravity = 9.8f;
    
    private float velocity = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(0, startHeight, 0);
    }
    
    void Update()
    {
        // 簡單的重力加速
        velocity += gravity * Time.deltaTime;
        
        // 向下移動
        transform.position += Vector3.down * velocity * Time.deltaTime;
        
        // 著地
        if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            velocity = 0;
        }
        
        // R 重置
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, startHeight, 0);
            velocity = 0;
        }
    }
}
