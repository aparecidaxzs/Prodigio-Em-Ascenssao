using UnityEngine;

public class PlayerRayInteraction : MonoBehaviour
{
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
        GameObject[] enemies = FindEnemiesInLayer("Inimigo");

        Debug.Log($"Encontrados {enemies.Length} objetos na layer Inimigo");

        int killed = 0;
        for (int i = 0; i < enemies.Length && killed < 3; i++)
        {
            if (enemies[i].CompareTag("Player") || 
                enemies[i].GetComponent<Player>() != null || 
                enemies[i].name.Contains("Player"))
            {
                Debug.Log($"Pulando {enemies[i].name} - é o Player");
                continue;
            }

            Debug.Log($"Tentando matar: {enemies[i].name}");

            // ---- AQUI: usa SendMessage para chamar Die() se existir (sem erro se não existir)
            enemies[i].SendMessage("Die", SendMessageOptions.DontRequireReceiver);

            // Se quiser garantir (caso EnemyAI só tenha TakeDamage), pode descomentar isto:
            // EnemyAI ea = enemies[i].GetComponent<EnemyAI>();
            // if (ea != null) { ea.TakeDamage(999); }

            killed++;
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
