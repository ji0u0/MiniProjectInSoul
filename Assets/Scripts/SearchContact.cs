using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// 딕셔너리(hash) + sliding window 알고리즘
// 장점 : 검색어를 포함하는 모든 주소록을 찾음 = 기능적으로 완성
// 단점 : 시간 복잡도 그대로...
public class SearchContact : MonoBehaviour
{
    [SerializeField] public InputField searchInput;
    [SerializeField] public Button searchButton;
    [SerializeField] public Text searchResultText;

    private Dictionary<string, List<Contact>> phoneBook = new Dictionary<string, List<Contact>>();

    // Start is called before the first frame update
    void Start()
    {
        AddContact(new Contact("kangjiwoo", "01092139670"));
        AddContact(new Contact("kangsumin", "01020182374"));
        AddContact(new Contact("kangjaewook", "01085129670"));
        AddContact(new Contact("kangsungil", "01088329228"));
        AddContact(new Contact("moonkyungran", "01064772374"));

        searchButton.onClick.AddListener(Search);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddContact(Contact contact)
    {
        // 해시가 없으면 생성
        if (!phoneBook.ContainsKey(contact.Name))
        {
            phoneBook[contact.Name] = new List<Contact>();
        }

        phoneBook[contact.Name].Add(contact); // 추가
    }

    public void Search()
    {
        string searchQuery = searchInput.text.Trim().ToLower();

        List<Contact> result = new List<Contact>();

        foreach (var contact in phoneBook)
        {
            if (SlidingWindowMatch(contact.Key, searchQuery))
            {
                result.AddRange(contact.Value);
            }
        }

        searchResultText.text = string.Join("\n", result.Select(contact => $"Name: {contact.Name}, Phone: {contact.PhoneNumber}"));
    }

    // 검색어가 해시 문자열에 포함되는지 확인하는 함수
    private bool SlidingWindowMatch(string text, string pattern)
    {
        int m = pattern.Length;
        int n = text.Length;

        for (int i = 0; i <= n - m; i++)
        {
            if (text.Substring(i, m) == pattern)
            {
                return true;
            }
        }

        return false;
    }
}
