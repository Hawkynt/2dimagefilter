#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Classes;
using Classes.ScriptActions;

using NUnit.Framework;

namespace ImageResizer.Tests {
  /// <summary>
  /// Covers <see cref="ScriptEngine"/>: action bookkeeping, the source/target promotion rules and
  /// the ownership contract its setters implement.
  /// </summary>
  [TestFixture]
  public class ScriptEngineTests {

    #region helpers

    /// <summary>A script action whose behaviour each test dictates.</summary>
    private sealed class StubAction : IScriptAction {
      public bool ChangesSourceImage { get; set; }
      public bool ChangesTargetImage { get; set; }
      public bool ProvidesNewGdiSource => false;
      public string PoolSourceKey => null;
      public Bitmap GdiSource => null;
      public Bitmap SourceImage { get; set; }
      public Bitmap TargetImage { get; set; }

      /// <summary>What the action assigns to the slots it claims to change.</summary>
      public Func<Bitmap> ProduceSource { get; set; }
      public Func<Bitmap> ProduceTarget { get; set; }

      public int Executions { get; private set; }

      /// <summary>The source the engine handed in, recorded at execution time.</summary>
      public Bitmap ObservedSource { get; private set; }
      public Bitmap ObservedTarget { get; private set; }

      public bool Execute() {
        ++this.Executions;
        this.ObservedSource = this.SourceImage;
        this.ObservedTarget = this.TargetImage;

        if (this.ChangesSourceImage && this.ProduceSource != null)
          this.SourceImage = this.ProduceSource();

        if (this.ChangesTargetImage && this.ProduceTarget != null)
          this.TargetImage = this.ProduceTarget();

        return true;
      }
    }

    /// <summary>Reports whether a bitmap has already been disposed.</summary>
    private static bool _IsDisposed(Bitmap bitmap) {
      try {
        var unused = bitmap.Width;
        return false;
      } catch (ArgumentException) {
        return true;
      } catch (ObjectDisposedException) {
        return true;
      }
    }

    private static StubAction _SourceProducer(Bitmap bitmap)
      => new StubAction { ChangesSourceImage = true, ProduceSource = () => bitmap }
    ;

    #endregion

    #region action list

    [Test]
    public void NewEngine_HasNoActionsAndNoImages() {
      var engine = new ScriptEngine();

      Assert.That(engine.Actions, Is.Empty);
      Assert.That(engine.SourceImage, Is.Null);
      Assert.That(engine.TargetImage, Is.Null);
    }

    [Test]
    public void AddWithoutExecution_QueuesButDoesNotRun() {
      var engine = new ScriptEngine();
      var action = new StubAction();

      engine.AddWithoutExecution(action);

      Assert.That(engine.Actions.Count(), Is.EqualTo(1));
      Assert.That(action.Executions, Is.Zero);
    }

    [Test]
    public void ExecuteAction_RunsAndRecords() {
      var engine = new ScriptEngine();
      var action = new StubAction();

      engine.ExecuteAction(action);

      Assert.That(action.Executions, Is.EqualTo(1));
      Assert.That(engine.Actions.Single(), Is.SameAs(action));
    }

    [Test]
    public void RepeatActions_RunsEveryQueuedActionInOrder() {
      var engine = new ScriptEngine();
      var order = new List<int>();
      for (var i = 0; i < 3; ++i) {
        var index = i;
        var action = new StubAction { ChangesTargetImage = true, ProduceTarget = () => { order.Add(index); return null; } };
        engine.AddWithoutExecution(action);
      }

      engine.RepeatActions();

      Assert.That(order, Is.EqualTo(new[] { 0, 1, 2 }));
    }

    [Test]
    public void RepeatActions_DoesNotGrowTheActionList() {
      var engine = new ScriptEngine();
      engine.AddWithoutExecution(new StubAction());

      engine.RepeatActions();

      Assert.That(engine.Actions.Count(), Is.EqualTo(1));
    }

    [Test]
    public void RepeatActions_CallsThePreAndPostHooksAroundEachAction() {
      var engine = new ScriptEngine();
      var action = new StubAction();
      engine.AddWithoutExecution(action);
      var log = new List<string>();

      engine.RepeatActions((e, c) => log.Add("pre"), (e, c) => log.Add("post"));

      Assert.That(log, Is.EqualTo(new[] { "pre", "post" }));
    }

    [Test]
    public void RepeatActions_WithoutHooks_Works()
      => Assert.That(() => new ScriptEngine().RepeatActions(), Throws.Nothing)
    ;

    [Test]
    public void Clear_EmptiesTheActionList() {
      var engine = new ScriptEngine();
      engine.AddWithoutExecution(new StubAction());

      engine.Clear();

      Assert.That(engine.Actions, Is.Empty);
    }

    [Test]
    public void Actions_IsASnapshotTheCallerCannotMutate() {
      var engine = new ScriptEngine();
      engine.AddWithoutExecution(new StubAction());

      Assert.That(engine.Actions, Is.Not.InstanceOf<ICollection<IScriptAction>>().And.Not.InstanceOf<IList<IScriptAction>>());
    }

    #endregion

    #region image promotion

    [Test]
    public void ActionThatChangesTheSource_PromotesItIntoTheEngine() {
      var engine = new ScriptEngine();
      var produced = TestBitmaps.Create();

      engine.ExecuteAction(_SourceProducer(produced));

      Assert.That(engine.SourceImage, Is.SameAs(produced));
      Assert.That(engine.IsSourceImageChanged, Is.True);
    }

    [Test]
    public void ActionThatChangesTheTarget_PromotesItIntoTheEngine() {
      var engine = new ScriptEngine();
      var produced = TestBitmaps.Create();

      engine.ExecuteAction(new StubAction { ChangesTargetImage = true, ProduceTarget = () => produced });

      Assert.That(engine.TargetImage, Is.SameAs(produced));
      Assert.That(engine.IsTargetImageChanged, Is.True);
    }

    [Test]
    public void ActionThatChangesNothing_LeavesTheSlotsAlone() {
      var engine = new ScriptEngine();
      var source = TestBitmaps.Create();
      engine.ExecuteAction(_SourceProducer(source));

      engine.ExecuteAction(new StubAction());

      Assert.That(engine.SourceImage, Is.SameAs(source));
      Assert.That(engine.IsSourceImageChanged, Is.False);
      Assert.That(engine.IsTargetImageChanged, Is.False);
    }

    [Test]
    public void EachAction_SeesTheCurrentSlotContents() {
      var engine = new ScriptEngine();
      var source = TestBitmaps.Create();
      engine.ExecuteAction(_SourceProducer(source));
      var observer = new StubAction();

      engine.ExecuteAction(observer);

      Assert.That(observer.ObservedSource, Is.SameAs(source));
    }

    [Test]
    public void GdiAliases_TrackTheSlots() {
      var engine = new ScriptEngine();
      var source = TestBitmaps.Create();

      engine.ExecuteAction(_SourceProducer(source));

      Assert.That(engine.GdiSource, Is.SameAs(engine.SourceImage));
      Assert.That(engine.GdiTarget, Is.SameAs(engine.TargetImage));
    }

    #endregion

    #region ownership

    [Test]
    public void ReplacingTheSource_DisposesTheOneItReplaces() {
      var engine = new ScriptEngine();
      var first = TestBitmaps.Create();
      engine.ExecuteAction(_SourceProducer(first));

      engine.ExecuteAction(_SourceProducer(TestBitmaps.Create()));

      Assert.That(_IsDisposed(first), Is.True);
    }

    [Test]
    public void ReplacingTheSourceWithItself_DoesNotDisposeIt() {
      var engine = new ScriptEngine();
      var bitmap = TestBitmaps.Create();
      engine.ExecuteAction(_SourceProducer(bitmap));

      engine.ExecuteAction(_SourceProducer(bitmap));

      Assert.That(_IsDisposed(bitmap), Is.False);
      Assert.That(engine.SourceImage, Is.SameAs(bitmap));
    }

    [Test]
    public void ReplacingTheTarget_DisposesTheOneItReplaces() {
      var engine = new ScriptEngine();
      var first = TestBitmaps.Create();
      engine.ExecuteAction(new StubAction { ChangesTargetImage = true, ProduceTarget = () => first });

      engine.ExecuteAction(new StubAction { ChangesTargetImage = true, ProduceTarget = () => TestBitmaps.Create() });

      Assert.That(_IsDisposed(first), Is.True);
    }

    [Test]
    public void PoolManagedSource_IsNotDisposedWhenReplaced() {
      var engine = new ScriptEngine();
      var pooled = TestBitmaps.Create();
      engine.SetSourceImageNonOwning(pooled, "key");

      engine.ExecuteAction(_SourceProducer(TestBitmaps.Create()));

      Assert.That(_IsDisposed(pooled), Is.False, "the pool owns it; disposing would corrupt every later checkout");
    }

    [Test]
    public void PoolManagedSource_ExposesItsKey() {
      var engine = new ScriptEngine();

      engine.SetSourceImageNonOwning(TestBitmaps.Create(), "some/file.png");

      Assert.That(engine.CurrentSourceKey, Is.EqualTo("some/file.png"));
    }

    [Test]
    public void EngineOwnedSource_HasNoPoolKey() {
      var engine = new ScriptEngine();

      engine.ExecuteAction(_SourceProducer(TestBitmaps.Create()));

      Assert.That(engine.CurrentSourceKey, Is.Null);
    }

    [Test]
    public void PromotingAnEngineOwnedSource_ClearsAPreviousPoolKey() {
      var engine = new ScriptEngine();
      engine.SetSourceImageNonOwning(TestBitmaps.Create(), "key");

      engine.ExecuteAction(_SourceProducer(TestBitmaps.Create()));

      Assert.That(engine.CurrentSourceKey, Is.Null);
    }

    #endregion

    #region revert

    [Test]
    public void RevertToLastSource_DropsTrailingActionsThatLeftTheSourceAlone() {
      var engine = new ScriptEngine();
      var sourceChanging = _SourceProducer(TestBitmaps.Create());
      engine.AddWithoutExecution(sourceChanging);
      engine.AddWithoutExecution(new StubAction { ChangesTargetImage = true });
      engine.AddWithoutExecution(new StubAction { ChangesTargetImage = true });

      engine.RevertToLastSource();

      Assert.That(engine.Actions.Single(), Is.SameAs(sourceChanging));
    }

    [Test]
    public void RevertToLastSource_KeepsAListThatEndsOnASourceChange() {
      var engine = new ScriptEngine();
      engine.AddWithoutExecution(new StubAction { ChangesTargetImage = true });
      engine.AddWithoutExecution(_SourceProducer(TestBitmaps.Create()));

      engine.RevertToLastSource();

      Assert.That(engine.Actions.Count(), Is.EqualTo(2));
    }

    [Test]
    public void RevertToLastSource_OnAnEmptyList_DoesNothing() {
      var engine = new ScriptEngine();

      Assert.That(() => engine.RevertToLastSource(), Throws.Nothing);
      Assert.That(engine.Actions, Is.Empty);
    }

    [Test]
    public void RevertToLastSource_WithNoSourceChangeAtAll_EmptiesTheList() {
      var engine = new ScriptEngine();
      engine.AddWithoutExecution(new StubAction { ChangesTargetImage = true });
      engine.AddWithoutExecution(new StubAction { ChangesTargetImage = true });

      engine.RevertToLastSource();

      Assert.That(engine.Actions, Is.Empty);
    }

    #endregion

  }
}
