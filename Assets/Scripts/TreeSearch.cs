using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 트라이(Trie) 알고리즘
// 장점 : 시간 복잡도 가장 적음 O(m) (m = 문자열 길이)
// 단점 : 접두사만 검색 가능
public class TrieSearch : MonoBehaviour
{
    PhoneBook phoneBook = new PhoneBook();

    [SerializeField] public InputField searchInput;
    [SerializeField] public Button searchButton;
    [SerializeField] public Text searchResultText;

    // Start is called before the first frame update
    void Start()
    {
        phoneBook.AddContact("kangjiwoo", "01092139670");
        phoneBook.AddContact("kangsumin", "01020182374");
        phoneBook.AddContact("kangjaewook", "01085129670");
        phoneBook.AddContact("kangsungil", "01088329228");
        phoneBook.AddContact("moonkyungran", "01064772374");

        searchButton.onClick.AddListener(SearchContact);

        var results = phoneBook.SearchContacts("Da");
        foreach (var result in results)
        {
            Debug.Log(result);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchContact()
    {
        string searchQuery = searchInput.text.Trim().ToLower();

        var result = phoneBook.SearchContacts(searchQuery);

        searchResultText.text = string.Join("\n", result.Select(contact => $"Name: {contact.Name}, Phone: {contact.PhoneNumber}"));
    }
}

public class PhoneBook
{
    private readonly TrieNode root = new TrieNode();

    public void AddContact(string name, string phoneNumber)
    {
        var currentNode = root;

        foreach (var ch in name)
        {
            // 현재 ch가 현재 node의 children에 없다면 새 node를 판다
            if (!currentNode.Children.ContainsKey(ch))
            {
                currentNode.Children[ch] = new TrieNode();
            }

            //  현재 node를 찾은 node(혹은 생성한 node)로 변경한다
            currentNode = currentNode.Children[ch];
        }
        
        // 단어의 끝임을 표시
        currentNode.IsEndOfWord = true;
        currentNode.PhoneNumber = phoneNumber;
    }

    public List<Contact> SearchContacts(string partialName)
    {
        var currentNode = root;

        // 검색어 노드가 존재하는 지 확인
        foreach (var ch in partialName)
        {
            if (!currentNode.Children.ContainsKey(ch))
            {
                return new List<Contact>();
            }

            currentNode = currentNode.Children[ch];
        }
        
        // 검색어 Node까지 도달했다면...
        return CollectContacts(currentNode, partialName);
    }

    // 접두사의 자식 노드를 collect하는 함수
    private List<Contact> CollectContacts(TrieNode node, string prefix)
    {
        var contacts = new List<Contact>();

        // 단어가 끝나면 리스트에 추가
        if (node.IsEndOfWord)
        {
            contacts.Add(new Contact(prefix, node.PhoneNumber));
        }

        // 단어가 끝날 때까지 자식 접두사와 자식 노드를 합친다
        foreach (var child in node.Children)
        {
            contacts.AddRange(CollectContacts(child.Value, prefix + child.Key));
        }

        return contacts;
    }
}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; } // 자식 노드를 저장하는 딕셔너리
    public bool IsEndOfWord { get; set; } // 단어의 끝인가?
    public string PhoneNumber { get; set; } // 전화번호 (끝이 아니면 null)

    public TrieNode()
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        PhoneNumber = null;
    }
}
