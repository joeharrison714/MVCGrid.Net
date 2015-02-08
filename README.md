# MVCGrid

## Features
* Uses your existing model objects
* Server-side sorting and paging using AJAX
* updates query string to support maintaining grid state when navigating back
* gracefully degrades on older browsers
* enable filtering with minimal code
* Built-in exporting to csv

## Usage

Add a mapping to you app start (Global.asax)

```
var grid = new GridDefinition<TestItem>(globalConfig)
	.WithColumn("Col1", "Col1", ((p, h) => p.Col1))
	.WithColumn("Col2", "Col2", ((p, h) => p.Col2))
	.WithColumn("Col3", "Col3", ((p, h) => p.Col3))
	.WithRetrieveData(((options) =>
	{
		TestItemRepository repo = new TestItemRepository();
		int totalRecords;
		var items = repo.GetData(out totalRecords, options.GetLimitOffset(), options.GetLimitRowcount(), options.SortColumn, options.SortDirection == SortDirection.Dsc);

		return new QueryResult<TestItem>()
		{
			Items = items,
			TotalRecords = totalRecords
		};
	}));
MVCGridMappingTable.Add("TestMapping", grid);
```

Add the following to your view to show the grid:
```
@(Html.MVCGrid("TestMapping"))
```
