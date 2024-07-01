using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedTrie : MonoBehaviour
{
    [SerializeField] public InputField searchInput;
    [SerializeField] public Button searchButton;
    [SerializeField] public Text searchResultText;

    PhoneBook phoneBook = new PhoneBook();

    // Start is called before the first frame update
    void Start()
    {
        phoneBook.AddContact("kangjiwoo", "01092139670");
        phoneBook.AddContact("kangsumin", "01020182374");
        phoneBook.AddContact("kangjaewook", "01085129670");
        phoneBook.AddContact("kangsungil", "01088329228");
        phoneBook.AddContact("moonkyungran", "01064772374");
        phoneBook.AddContact("leeyoungho", "01012345678");
        phoneBook.AddContact("kimjiyoung", "01023456789");
        phoneBook.AddContact("parkminsoo", "01034567890");
        phoneBook.AddContact("choiheesun", "01045678901");
        phoneBook.AddContact("yoonhyunwoo", "01056789012");
        phoneBook.AddContact("jangdongmin", "01067890123");
        phoneBook.AddContact("shimchangmin", "01078901234");
        phoneBook.AddContact("ohsehun", "01089012345");
        phoneBook.AddContact("jungyumi", "01090123456");
        phoneBook.AddContact("kwonhyojin", "01091234567");
        phoneBook.AddContact("baekhyunjoo", "01010345678");
        phoneBook.AddContact("sunwoojung", "01011456789");
        phoneBook.AddContact("imsoojin", "01012567890");
        phoneBook.AddContact("namdonghyun", "01013678901");
        phoneBook.AddContact("hanchaeun", "01014789012");
        phoneBook.AddContact("soryujin", "01015890123");
        phoneBook.AddContact("ryuseojin", "01016901234");
        phoneBook.AddContact("kojihoon", "01017012345");
        phoneBook.AddContact("chaejoon", "01018123456");
        phoneBook.AddContact("ahnsoyoung", "01019234567");
    }

    // Update is called once per frame
    void Update()
    {
        searchButton.onClick.AddListener(SearchContact);
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
        // 문자열의 모든 부분을 Trie로 만들기
        for (int i = 0; i < name.Length; i++)
        {
            var currentNode = root;
            for (int j = i; j < name.Length; j++)
            {
                char ch = name[j];

                // 현재 ch가 현재 node의 children에 없다면 새 node를 판다
                if (!currentNode.Children.ContainsKey(ch))
                {
                    currentNode.Children[ch] = new TrieNode();
                }

                // 현재 node를 찾은 node(혹은 생성한 node)로 변경한다
                currentNode = currentNode.Children[ch];
            }
        
            // 단어의 끝임을 표시
            currentNode.IsEndOfWord = true;
            
            if (currentNode.Contacts == null)
            {
                currentNode.Contacts = new List<Contact>();
            }

            // 연락처 추가
            currentNode.Contacts.Add(new Contact(name, phoneNumber));
        }
    }

    public List<Contact> SearchContacts(string partialName)
    {
        var result = new List<Contact>();
        var currentNode = root;

        // 검색어 노드가 존재하는지 확인
        foreach (var ch in partialName)
        {
            if (!currentNode.Children.ContainsKey(ch))
            {
                return result;
            }

            currentNode = currentNode.Children[ch];
        }
        
        // 검색어 Node까지 도달했다면...
        result = CollectContacts(currentNode);
        return result;
    }

    // 자식 노드의 contact를 모두 collect하는 함수
    private List<Contact> CollectContacts(TrieNode node)
    {
        var contacts = new List<Contact>();

        // 단어가 끝나면 리스트에 추가
        if (node.IsEndOfWord)
        {
            contacts.AddRange(node.Contacts);
        }

        // 단어가 끝날 때까지 자식 접두사와 자식 노드를 합친다
        foreach (var child in node.Children)
        {
            contacts.AddRange(CollectContacts(child.Value));
        }

        return contacts;
    }
}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; } // 자식 노드를 저장하는 딕셔너리
    public bool IsEndOfWord { get; set; } // 단어의 끝인가?
    public List<Contact> Contacts { get; set; } // 연락처

    public TrieNode()
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        Contacts = null;
    }
}
