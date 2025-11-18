using UnityEngine;

public class PlayerRayInteraction : MonoBehaviour
{
    // Função chamada quando o Player entra no trigger do Raio
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Raio"))
        {
            Debug.Log("Player tocou no raio - destruindo raio e matando inimigos");
            KillEnemies();
            Destroy(other.gameObject); // Destrói o raio imediatamente
        }
    }

    private void KillEnemies()
    {
        // Encontra todos os inimigos na layer "Inimigo"
        GameObject[] enemies = FindEnemiesInLayer("Inimigo");

        Debug.Log($"Encontrados {enemies.Length} objetos na layer Inimigo");

        // Mata os primeiros 3 (ou menos se houver menos), mas ignora o Player
        int killed = 0;
        for (int i = 0; i < enemies.Length && killed < 3; i++)
        {
            // Verifica se NÃO é o Player (por tag, componente ou nome)
            if (enemies[i].CompareTag("Player") || 
                enemies[i].GetComponent<Player>() != null || 
                enemies[i].name.Contains("Player")) // Extra: verifica nome
            {
                Debug.Log($"Pulando {enemies[i].name} - é o Player");
                continue; // Pula o Player
            }

            Debug.Log($"Matando inimigo: {enemies[i].name}");
            EnemyAI enemyScript = enemies[i].GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                enemyScript.Die();
                killed++;
            }
            else
            {
                Destroy(enemies[i]);
                killed++;
            }
        }
        Debug.Log($"Total inimigos mortos: {killed}");
    }

    private GameObject[] FindEnemiesInLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) 
        {
            Debug.LogError($"Layer '{layerName}' não encontrada!");
            return new GameObject[0];
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        return System.Array.FindAll(allObjects, obj => obj.layer == layer);
    }
}